using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using GameMasterCore.Components.Factories;
using GameMasterCore.Components.Modules;
using PlayerCore.Components.Factories;
using PlayerCore.Components.Modules;
using Shared.Components.Events;
using Shared.Components.Serialization;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationSubstitute
{
	class Program
	{
		public static async Task Main( string[] args )
		{
			try
			{
				//RejectGameRegistration rejectGameRegistration = new RejectGameRegistration() { GameName = null };
				//Console.WriteLine( Serializer.Serialize( rejectGameRegistration ) );
				//RejectGameRegistration rejectGameRegistration2 = Serializer.Deserialize<RejectGameRegistration>( Serializer.Serialize( rejectGameRegistration ) );
				//Console.WriteLine( Serializer.Serialize( rejectGameRegistration2 ) );
				string ip = "127.0.0.1";
				int port = 65535;
				var gameMasterSettings = new GameMasterSettings
				{
					GameDefinition = new GameMasterSettingsGameDefinition
					{
						GameName = "Test Game",
						NumberOfPlayersPerTeam = 1u
					}
				};
				int timeout = 20000;
				using( var cts = new CancellationTokenSource( timeout ) )
				{
					var cs = new CommunicationServerModule( ip, port, new CommunicationServerSettings(), new CommunicationServerFactory() );
					var gm1 = new GameMasterModule( ip, port, gameMasterSettings, new GameMasterFactory() );
					var p1 = new PlayerModule( ip, port, new PlayerSettings(), new PlayerFactory() );
					var p2 = new PlayerModule( ip, port, new PlayerSettings(), new PlayerFactory() );
					cs.Sent += OnSent;
					cs.Received += OnReceived;
					cs.SentKeepAlive += OnSentKeepAlive;
					cs.ReceivedKeepAlive += OnReceivedKeepAlive;
					cs.Discarded += OnDiscarded;
					gm1.Sent += OnSent;
					gm1.Received += OnReceived;
					gm1.SentKeepAlive += OnSentKeepAlive;
					gm1.ReceivedKeepAlive += OnReceivedKeepAlive;
					gm1.Discarded += OnDiscarded;
					var tasks = new List<Task>
					{
						Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await gm1.RunAsync( cts.Token ).ConfigureAwait( false ) )//,
						//Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						//Task.Run( async () => await p2.RunAsync( cts.Token ).ConfigureAwait( false ) )
					};
					try
					{
						await Task.WhenAll( tasks );
					}
					catch( OperationCanceledException )
					{
					}
					finally
					{
						foreach( var task in tasks )
						{
							if( task.IsFaulted )
								Console.WriteLine( $"Module task faulted with { task.Exception.GetType().Name }." );
							else if( task.IsCanceled )
								Console.WriteLine( $"Module task canceled." );
							else
								Console.WriteLine( "Module task completed." );
						}
					}
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( e );//.GetType().Name );
			}
		}
		private static void OnSent( object s, SentArgs e )
		{
			Console.WriteLine( $"{ e.Local } sends to { e.Remote }:" );
			Console.WriteLine( e.SerializedMessage );
		}
		private static void OnReceived( object s, ReceivedArgs e )
		{
			Console.WriteLine( $"{ e.Local } received from { e.Remote }:" );
			Console.WriteLine( e.SerializedMessage );
		}
		private static void OnSentKeepAlive( object s, SentKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } sends keep alive to { e.Remote }:" );
		private static void OnReceivedKeepAlive( object s, ReceivedKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } received keep alive from { e.Remote }:" );
		private static void OnDiscarded( object s, DiscardedArgs e )
		{
			Console.WriteLine( $"{ e.Local } discarded message from { e.Remote }:" );
			Console.WriteLine( e.SerializedMessage );
		}
	}
}
