using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using CommunicationServerCore.Components.Options;
using Shared.Components.Events;
using Shared.Components.Exceptions;
using Shared.Components.Options;
using Shared.Interfaces.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore
{
	class Program
    {
        public static async Task Main( string[] args )
        {
			try
			{
				var options = new CommunicationServerOptions( args );
				using( var cts = new CancellationTokenSource() )
				{
					var module = new CommunicationServerModule( "localhost", options.Port, options.Conf, new CommunicationServerFactory() );
					Debug( module );
					CommandLineOptions.CancelOnCtrlC( cts );
					var task = Task.Run( async () => await module.RunAsync( cts.Token ).ConfigureAwait( false ) );
					try
					{
						await task;
					}
					catch( OperationCanceledException )
					{
					}
					finally
					{
						if( task.IsFaulted )
						{
							Console.WriteLine( "Module task faulted by:" );
							foreach( var e in task.Exception.Flatten().InnerExceptions )
								if( e is DisconnectionException )
									Console.WriteLine( $"Disconnection ( { e.Message } )" );
								else
									Console.WriteLine( $"Exception: { e }." );
						}
						else if( task.IsCanceled )
							Console.WriteLine( $"Module task canceled." );
						else
							Console.WriteLine( "Module task completed." );
					}
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( e );
			}
		}
		private static void Debug( ICommunicationObserver communicationObserver )
		{
			communicationObserver.Sent += OnSent;
			communicationObserver.Received += OnReceived;
			//communicationObserver.SentKeepAlive += OnSentKeepAlive;
			//communicationObserver.ReceivedKeepAlive += OnReceivedKeepAlive;
			communicationObserver.Discarded += OnDiscarded;
			communicationObserver.Disconnected += OnDisconnected;
		}
		private static void OnSent( object s, SentArgs e ) => Console.WriteLine( $"{ e.Local } sends to { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnReceived( object s, ReceivedArgs e ) => Console.WriteLine( $"{ e.Local } received from { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnSentKeepAlive( object s, SentKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } sends keep alive to { e.Remote }.\n" );
		private static void OnReceivedKeepAlive( object s, ReceivedKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } received keep alive from { e.Remote }.\n" );
		private static void OnDiscarded( object s, DiscardedArgs e ) => Console.WriteLine( $"{ e.Local } discarded message from { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnDisconnected( object s, DisconnectedArgs e ) => Console.WriteLine( $"{ e.Local } lost connection with { e.Remote }.\n" );
	}
}
