using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using GameMasterCore.Components.Factories;
using GameMasterCore.Components.Modules;
using PlayerCore.Components.Factories;
using PlayerCore.Components.Modules;
using Shared.Components.Events;
using Shared.Components.Exceptions;
using Shared.DTO.Configuration;
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
						NumberOfPlayersPerTeam = 3u,
                        Goals = new List<GoalField>
                        {
                            new GoalField { Team = TeamColour.Blue, X = 3, Y = 0, Type = GoalFieldType.Goal },
                            new GoalField { Team = TeamColour.Red, X = 3, Y = 12, Type = GoalFieldType.Goal },
                            new GoalField { Team = TeamColour.Blue, X = 0, Y = 0, Type = GoalFieldType.Goal },
                            new GoalField { Team = TeamColour.Red, X = 0, Y = 12, Type = GoalFieldType.Goal }
                        }
                    }
				};
				int timeout = -1;
				using( var cts = new CancellationTokenSource( timeout ) )
				{
					var cs = new CommunicationServerModule( ip, port, new CommunicationServerSettings(), new CommunicationServerFactory() );
					var gm1 = new GameMasterModule( ip, port, gameMasterSettings, new GameMasterFactory() );
					var p1 = new PlayerModule( ip, port, new PlayerSettings(), gameName, TeamColour.Blue, PlayerRole.Leader, new PlayerFactory() );
					var p2 = new PlayerModule( ip, port, new PlayerSettings(), gameName, TeamColour.Red, PlayerRole.Leader, new PlayerFactory() );
				    var p3 = new PlayerModule(ip, port, new PlayerSettings(), gameName, TeamColour.Blue, PlayerRole.Member, new PlayerFactory());
				    var p4 = new PlayerModule(ip, port, new PlayerSettings(), gameName, TeamColour.Red, PlayerRole.Member, new PlayerFactory());
				    var p5 = new PlayerModule(ip, port, new PlayerSettings(), gameName, TeamColour.Blue, PlayerRole.Member, new PlayerFactory());
				    var p6 = new PlayerModule(ip, port, new PlayerSettings(), gameName, TeamColour.Red, PlayerRole.Member, new PlayerFactory());
                    Debug( cs );
					Debug( gm1 );
					Debug( p1 );
					Debug( p2 );
                    Debug(p3);
                    Debug(p4);
                    Debug(p5);
                    Debug(p6);
					CancelOnCtrlC( cts );
					var tasks = new List<Task>
					{
						Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await gm1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await p2.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					    Task.Run( async () => await p3.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					    Task.Run( async () => await p4.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					    Task.Run( async () => await p5.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					    Task.Run( async () => await p6.RunAsync( cts.Token ).ConfigureAwait( false ) )
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
