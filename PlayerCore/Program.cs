using PlayerCore.Components.Factories;
using PlayerCore.Components.Modules;
using Shared.Components.Events;
using Shared.Components.Exceptions;
using Shared.DTOs.Configuration;
using Shared.Enums;
using Shared.Interfaces.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shared.Components.ArgumentOptions;

namespace PlayerCore
{
	class Program
    {
		public static async Task Main( string[] args )
		{
			try
			{
                var po = new PlayerOptions(ArgumentOptionsHelper.GetDictonary(args));
			    var playerSettings = ArgumentOptionsHelper.GetConfigFile<PlayerSettings>(po.Conf);
				using( var cts = new CancellationTokenSource() )
				{
					var module = new PlayerModule( po.Address, po.Port, playerSettings, po.Game, po.Team, po.Role, new PlayerFactory() );
					Debug( module );
					CancelOnCtrlC( cts );
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
			communicationObserver.SentKeepAlive += OnSentKeepAlive;
			communicationObserver.ReceivedKeepAlive += OnReceivedKeepAlive;
			communicationObserver.Discarded += OnDiscarded;
			communicationObserver.Disconnected += OnDisconnected;
		}
		private static void OnSent( object s, SentArgs e ) => Console.WriteLine( $"{ e.Local } sends to { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnReceived( object s, ReceivedArgs e ) => Console.WriteLine( $"{ e.Local } received from { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnSentKeepAlive( object s, SentKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } sends keep alive to { e.Remote }.\n" );
		private static void OnReceivedKeepAlive( object s, ReceivedKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } received keep alive from { e.Remote }.\n" );
		private static void OnDiscarded( object s, DiscardedArgs e ) => Console.WriteLine( $"{ e.Local } discarded message from { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnDisconnected( object s, DisconnectedArgs e ) => Console.WriteLine( $"{ e.Local } lost connection with { e.Remote }.\n" );
		private static void CancelOnCtrlC( CancellationTokenSource cts ) => Console.CancelKeyPress += ( s, e ) =>
		{
			if( e.SpecialKey == ConsoleSpecialKey.ControlC )
			{
				e.Cancel = true;
				cts.Cancel();
			}
		};
	}
}
