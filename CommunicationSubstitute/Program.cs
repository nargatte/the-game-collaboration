using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using GameMasterCore.Components.Factories;
using GameMasterCore.Components.Modules;
using PlayerCore.Components.Factories;
using PlayerCore.Components.Modules;
using Shared.Components.Events;
using Shared.Components.Exceptions;
using Shared.DTOs.Configuration;
using Shared.Enums;
using Shared.Interfaces.Events;
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
				string ip = "127.0.0.1";
				int port = 65535;
				string gameName = "Test Game";
				var gameMasterSettings = new GameMasterSettings
				{
					GameDefinition = new GameMasterSettingsGameDefinition
					{
						GameName = gameName,
						NumberOfPlayersPerTeam = 1u,
                        Goals = new GoalField[]
                        {
                            new GoalField { Team = TeamColour.Blue, X = 0, Y = 0, Type = GoalFieldType.Goal},
                            new GoalField { Team = TeamColour.Red, X = 0, Y = 9, Type = GoalFieldType.Goal}
                        }
                    }
				};
				int timeout = 10000;
				using( var cts = new CancellationTokenSource(timeout) )
				{
					var cs = new CommunicationServerModule( ip, port, new CommunicationServerSettings(), new CommunicationServerFactory() );
					var gm1 = new GameMasterModule( ip, port, gameMasterSettings, new GameMasterFactory() );
					var p1 = new PlayerModule( ip, port, new PlayerSettings(), gameName, TeamColour.Blue, PlayerType.Leader, new PlayerFactory() );
					var p2 = new PlayerModule( ip, port, new PlayerSettings(), gameName, TeamColour.Red, PlayerType.Leader, new PlayerFactory() );
					//Debug( cs );
					//Debug( gm1 );
					Debug( p1 );
					var tasks = new List<Task>
					{
						Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await gm1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await p2.RunAsync( cts.Token ).ConfigureAwait( false ) )
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
							{
								Console.WriteLine( "Module task faulted by:" );
								foreach( var e in task.Exception.Flatten().InnerExceptions )
									if( e is DisconnectionException )
										Console.WriteLine( "Disconnection." );
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
			}
			catch( Exception )
			{
			}
		}
		private static void Debug( ICommunicationObserver communicationObserver )
		{
			communicationObserver.Sent += OnSent;
			communicationObserver.Received += OnReceived;
			//communicationObserver.SentKeepAlive += OnSentKeepAlive;
			//communicationObserver.ReceivedKeepAlive += OnReceivedKeepAlive;
			communicationObserver.Discarded += OnDiscarded;
		}
		private static void OnSent( object s, SentArgs e ) => Console.WriteLine( $"{ e.Local } sends to { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnReceived( object s, ReceivedArgs e ) => Console.WriteLine( $"{ e.Local } received from { e.Remote }:\n{ e.SerializedMessage }\n" );
		private static void OnSentKeepAlive( object s, SentKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } sends keep alive to { e.Remote }.\n" );
		private static void OnReceivedKeepAlive( object s, ReceivedKeepAliveArgs e ) => Console.WriteLine( $"{ e.Local } received keep alive from { e.Remote }.\n" );
		private static void OnDiscarded( object s, DiscardedArgs e ) => Console.WriteLine( $"{ e.Local } discarded message from { e.Remote }:\n{ e.SerializedMessage }\n" );
	}
}
