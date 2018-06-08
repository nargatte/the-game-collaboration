using CommunicationServerCore.Base.Servers;
using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Components.Exceptions;
using Shared.Components.Extensions;
using Shared.Const;
using Shared.DTO.Communication;
using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : CommunicationServerBase
	{
		#region CommunicationServerBase
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			var tasks = new List<Task>();
			Exception ex = null;
			try
			{
				using( var server = Factory.MakeNetworkServer( Ip, Port ) )
				{
					if( server is null )
						throw new NotImplementedException( nameof( Factory ) );
					while( true )
					{
						var client = await server.AcceptAsync( cancellationToken ).ConfigureAwait( false );
						tasks.Add( Task.Run( async () => await OnAcceptAsync( client, cancellationToken ).ConfigureAwait( false ) ) );
					}
				}
			}
			catch( OperationCanceledException )
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
			catch( Exception e )
			{
				cancellationToken.ThrowIfCancellationRequested();
				ex = e;
			}
			finally
			{
				try
				{
					Console.WriteLine( $"Server waits for completion." );
					await Task.WhenAll( tasks );
				}
				catch( OperationCanceledException )
				{
					cancellationToken.ThrowIfCancellationRequested();
					if( ex != null )
						throw new AggregateException( ex );
				}
				catch( Exception e )
				{
					cancellationToken.ThrowIfCancellationRequested();
					throw ex is null ? new AggregateException( e ) : new AggregateException( ex, e );
				}
				finally
				{
					foreach( var task in tasks )
					{
						if( task.IsFaulted )
						{
							Console.WriteLine( "Server task faulted by:" );
							foreach( var e in task.Exception.Flatten().InnerExceptions )
								if( e is DisconnectionException )
									Console.WriteLine( $"Disconnection ( { e.Message } )" );
								else
									Console.WriteLine( $"Exception: { e }." );
						}
						else if( task.IsCanceled )
							Console.WriteLine( $"Server task canceled." );
						else
							Console.WriteLine( "Server task completed." );
					}
				}
			}
			cancellationToken.ThrowIfCancellationRequested();
		}
		#endregion
		#region CommunicationServer
		private long nextPlayerId = ( long )Constants.AnonymousId;
		private long nextGameId = ( long )Constants.AnonymousId;
		private ConcurrentDictionary<string, IGameSession> gamesByName = new ConcurrentDictionary<string, IGameSession>();
		private ConcurrentDictionary<ulong, IGameSession> gamesById = new ConcurrentDictionary<ulong, IGameSession>();
		private ConcurrentDictionary<ulong, IPlayerSession> players = new ConcurrentDictionary<ulong, IPlayerSession>();
		public CommunicationServer( string ip, int port, uint keepAliveInterval, ICommunicationServerFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected async Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )//when new client connected
		{
			using( var cts = CancellationTokenSource.CreateLinkedTokenSource( cancellationToken ) )
			{
				try
				{
					using( var proxy = Factory.CreateClientProxy( client, KeepAliveInterval, cts.Token, Factory.MakeIdentity() ) )//make proxy for this unknown client
					{
						cts.Token.ThrowIfCancellationRequested();
						PassAll( proxy );//pass events from proxy
						proxy.Disconnected += ( s, e ) =>
						{
							cts.Cancel();
						};
						GetGames getGames = null;
						RegisterGame registerGame = null;
						while( proxy.Remote.Type is HostType.Unknown )//while cannot identify client
						{
							if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cts.Token ).ConfigureAwait( false ) ) != null )//check for GetGames
							{
								proxy.UpdateRemote( Factory.MakeIdentity( HostType.Player ) );
								await AsAnonymousPlayerAsync( proxy, getGames, cts.Token ).ConfigureAwait( false );//continue as anonymous Player
								break;
							}
							else if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cts.Token ).ConfigureAwait( false ) ) != null )//check for RegisterGame
							{
								proxy.UpdateRemote( Factory.MakeIdentity( HostType.GameMaster ) );
								await AsAnonymousGameMasterAsync( proxy, registerGame, cts.Token ).ConfigureAwait( false );//continue as anonymous GameMaster
								break;
							}
							else//doesn't matter
								proxy.Discard();
						}
					}
				}
				catch( OperationCanceledException )
				{
					if( !cancellationToken.IsCancellationRequested )
						throw new DisconnectionException( "Failed to keep alive." );
					else
						throw;
				}
			}
		}
		protected async Task AsAnonymousPlayerAsync( IClientProxy proxy, GetGames getGames, CancellationToken cancellationToken )//when Player is anonymous
		{
			cancellationToken.ThrowIfCancellationRequested();
			await GetGamesAsync( proxy, getGames, cancellationToken );//process request
			try
			{
				while( proxy.Remote.Id is Constants.AnonymousId )//while Player is anonymous
				{
					JoinGame joinGame;
					if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for GetGames
						await GetGamesAsync( proxy, getGames, cancellationToken );//process request
					else if( ( joinGame = await proxy.TryReceiveAsync<JoinGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for JoinGame
						await JoinGameAnonymousAsync( proxy, joinGame, cancellationToken );//process request
					else//doesn't matter
						proxy.Discard();
				}
				await AsRegisteredPlayerAsync( proxy, cancellationToken );//continue as registered Player
			}
			finally//unregister Player
			{
				if( proxy.Remote.Id != Constants.AnonymousId )
					players.TryRemove( proxy.Remote.Id, out var _ );
			}
		}
		protected async Task GetGamesAsync( IClientProxy proxy, GetGames getGames, CancellationToken cancellationToken )//when GetGames is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			var registeredGames = new RegisteredGames
			{
				GameInfo = ( from game in gamesById where game.Value.GameInfo != null select game.Value.GameInfo ).ToList()
			};
			await proxy.SendAsync( registeredGames, cancellationToken );
		}
		protected async Task JoinGameAnonymousAsync( IClientProxy proxy, JoinGame joinGame, CancellationToken cancellationToken )//when JoinGame is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			ulong id = ( ulong )Interlocked.Increment( ref nextPlayerId );//generate new player id
			proxy.UpdateRemote( Factory.CreateIdentity( HostType.Player, id ) );//set Player id
			players.TryAdd( id, Factory.CreatePlayerSession( proxy ) );//from now Player is visible to others
			joinGame.PlayerId = proxy.Remote.Id;
			await JoinGameRegisteredAsync( proxy, joinGame, cancellationToken );//continue registered
		}
		protected async Task JoinGameRegisteredAsync( IClientProxy proxy, JoinGame joinGame, CancellationToken cancellationToken )//when JoinGame is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( gamesByName.TryGetValue( joinGame.GameName, out var game ) )//if game exists
				try
				{
                    if( !joinGame.PlayerIdSpecified )
                        joinGame.PlayerId = proxy.Remote.Id;
					await game.GameMaster.SendAsync( joinGame, cancellationToken );
				}
				catch( Exception )//GameMaster fault
				{
				}
			else//if game doesn't exist
			{
				var rejectJoiningGame = new RejectJoiningGame
				{
					GameName = joinGame.GameName,
					PlayerId = joinGame.PlayerId
				};
				await proxy.SendAsync( rejectJoiningGame, cancellationToken );
			}
		}
		protected async Task AsRegisteredPlayerAsync( IClientProxy proxy, CancellationToken cancellationToken )//when Player is registered
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				while( true )
				{
					GetGames getGames;
					JoinGame joinGame;
					Discover discover;
					Move move;
					PickUpPiece pickUpPiece;
					TestPiece testPiece;
					PlacePiece placePiece;
					DestroyPiece destroyPiece;
					AuthorizeKnowledgeExchange authorizeKnowledgeExchange;
					RejectKnowledgeExchange rejectKnowledgeExchange;
					SuggestAction suggestAction;
					SuggestActionResponse suggestActionResponse;
					if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for GetGames
						await GetGamesAsync( proxy, getGames, cancellationToken );//process request
					else if( ( joinGame = await proxy.TryReceiveAsync<JoinGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for JoinGame
						await JoinGameRegisteredAsync( proxy, joinGame, cancellationToken );//process request
					else if( ( discover = await proxy.TryReceiveAsync<Discover>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for Discover
						await PassGameMessageAsync( proxy, discover, cancellationToken );//pass message
					else if( ( move = await proxy.TryReceiveAsync<Move>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for Move
						await PassGameMessageAsync( proxy, move, cancellationToken );//pass message
					else if( ( pickUpPiece = await proxy.TryReceiveAsync<PickUpPiece>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for PickUpPiece
						await PassGameMessageAsync( proxy, pickUpPiece, cancellationToken );//pass message
					else if( ( testPiece = await proxy.TryReceiveAsync<TestPiece>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for TestPiece
						await PassGameMessageAsync( proxy, testPiece, cancellationToken );//pass message
					else if( ( placePiece = await proxy.TryReceiveAsync<PlacePiece>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for PlacePiece
						await PassGameMessageAsync( proxy, placePiece, cancellationToken );//pass message
					else if( ( destroyPiece = await proxy.TryReceiveAsync<DestroyPiece>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for DestroyPiece
						await PassGameMessageAsync( proxy, destroyPiece, cancellationToken );//pass message
					else if( ( authorizeKnowledgeExchange = await proxy.TryReceiveAsync<AuthorizeKnowledgeExchange>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for AuthorizeKnowledgeExchange
						await PassGameMessageAsync( proxy, authorizeKnowledgeExchange, cancellationToken );//pass message
					else if( ( rejectKnowledgeExchange = await proxy.TryReceiveAsync<RejectKnowledgeExchange>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RejectKnowledgeExchange
						await RejectKnowledgeExchangeAsync( proxy, rejectKnowledgeExchange, cancellationToken );//pass message
					else if( ( suggestAction = await proxy.TryReceiveAsync<SuggestAction>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for SuggestAction
						await SuggestActionAsync( proxy, suggestAction, cancellationToken );//pass message
					else if( ( suggestActionResponse = await proxy.TryReceiveAsync<SuggestActionResponse>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for SuggestActionResponse
						await SuggestActionResponseAsync( proxy, suggestActionResponse, cancellationToken );//pass message
					else//doesn't matter
						proxy.Discard();
				}
			}
			catch( Exception )//disconnect Player
			{
				var playerDisconnected = new PlayerDisconnected
				{
					PlayerId = proxy.Remote.Id
				};
				players.TryGetValue( proxy.Remote.Id, out var session );
				if( session.GameId != Constants.AnonymousId )
				{
					if( gamesById.TryGetValue( session.GameId, out var game ) )
					{
						game.Players.TryRemove( proxy.Remote.Id, out ulong _ );
						try
						{
							await game.GameMaster.SendAsync( playerDisconnected, cancellationToken );
						}
						catch( Exception )//GameMaster fault
						{
						}
					}
					//else GameMaster fault
				}
				throw;
			}
		}
		protected async Task PassGameMessageAsync< T >( IClientProxy proxy, T gameMessage, CancellationToken cancellationToken ) where T : GameMessage//pass to registered GameMaster
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( gamesById.TryGetValue( gameMessage.GameId, out var game ) )//if game exists
				try
				{
					await game.GameMaster.SendAsync( gameMessage, cancellationToken );
				}
				catch( Exception )//GameMaster fault
				{
				}
			//else Player fault
		}
		protected async Task RejectKnowledgeExchangeAsync( IClientProxy proxy, RejectKnowledgeExchange rejectKnowledgeExchange, CancellationToken cancellationToken )//when RejectKnowlegdeExchange is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			players.TryGetValue( proxy.Remote.Id, out var session );
			if( session.GameId != Constants.AnonymousId )
			{
				if( gamesById.TryGetValue( session.GameId, out var game ) )
				{
					try
					{
						await game.GameMaster.SendAsync( rejectKnowledgeExchange, cancellationToken );
					}
					catch( Exception )//GameMaster fault
					{
					}
				}
				//else Player fault
			}
		}
		protected async Task SuggestActionAsync( IClientProxy proxy, SuggestAction suggestAction, CancellationToken cancellationToken )//when SuggestAction is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( gamesById.TryGetValue( suggestAction.GameId, out var game ) )//if game exists
				try
				{
					await game.GameMaster.SendAsync( suggestAction, cancellationToken );
				}
				catch( Exception )//GameMaster fault
				{
				}
			//else Player fault
		}
		protected async Task SuggestActionResponseAsync( IClientProxy proxy, SuggestActionResponse suggestActionResponse, CancellationToken cancellationToken )//when SuggestActionResponse is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( gamesById.TryGetValue( suggestActionResponse.GameId, out var game ) )//if game exists
				try
				{
					await game.GameMaster.SendAsync( suggestActionResponse, cancellationToken );
				}
				catch( Exception )//GameMaster fault
				{
				}
			//else Player fault
		}
		protected async Task AsAnonymousGameMasterAsync( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )//when GameMaster is anonymous
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				await RegisterGameAsync( proxy, registerGame, cancellationToken );
				while( proxy.Remote.Id is Constants.AnonymousId )//while GameMaster is anonymous
				{
					if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RegisterGame
						await RegisterGameAsync( proxy, registerGame, cancellationToken );//process request
					else//doesn't matter
						proxy.Discard();
				}
				await AsRegisteredGameMasterAsync( proxy, cancellationToken );//continue as registered GameMaster
			}
			finally//unregister GameMaster
			{
				if( gamesById.TryGetValue( proxy.Remote.Id, out var game ) )
				{
					gamesByName.TryRemove( game.Name, out var _ );
					gamesById.TryRemove( proxy.Remote.Id, out var _ );
				}
			}
		}
		protected async Task RegisterGameAsync( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )//when RegisterGame is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			string name = registerGame.NewGameInfo?.GameName;
			try
			{
				IGameSession game;
				if( name is null || !gamesByName.TryAdd( name, game = Factory.CreateGameSession( name, registerGame.NewGameInfo, proxy ) ) )//if cannot register new game
				{
					var rejectGameRegistration = new RejectGameRegistration
					{
						GameName = name
					};
					await proxy.SendAsync( rejectGameRegistration, cancellationToken );
				}
				else//if registered new game
				{
					ulong id = ( ulong )Interlocked.Increment( ref nextGameId );//generate new game id
					proxy.UpdateRemote( Factory.CreateIdentity( HostType.GameMaster, id ) );//set GameMaster id
					gamesById.TryAdd( id, game );//from now GameMaster is visible to others
					var confirmGameRegistration = new ConfirmGameRegistration
					{
						GameId = id
					};
					await proxy.SendAsync( confirmGameRegistration, cancellationToken );
				}
			}
			catch( Exception )//unregister GameMaster
			{
				gamesByName.TryRemove( name, out var _ );
				gamesById.TryRemove( proxy.Remote.Id, out var _ );
				throw;
			}
		}
		protected async Task AsRegisteredGameMasterAsync( IClientProxy proxy, CancellationToken cancellationToken )//when GameMaster is registered
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				while( true )
				{
					ConfirmJoiningGame confirmJoiningGame;
					RejectJoiningGame rejectJoiningGame;
					Game game;
					GameStarted gameStarted;
					Data data;
					KnowledgeExchangeRequest knowledgeExchangeRequest;
					RejectKnowledgeExchange rejectKnowledgeExchange;
					SuggestAction suggestAction;
					SuggestActionResponse suggestActionResponse;
					if( ( confirmJoiningGame = await proxy.TryReceiveAsync<ConfirmJoiningGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for ConfirmJoiningGame
						await ConfirmJoiningGameAsync( proxy, confirmJoiningGame, cancellationToken );//process request
					else if( ( rejectJoiningGame = await proxy.TryReceiveAsync<RejectJoiningGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RejectJoiningGame
						await PassPlayerMessageAsync( proxy, rejectJoiningGame, cancellationToken );//pass message
					else if( ( game = await proxy.TryReceiveAsync<Game>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for Game
						await PassPlayerMessageAsync( proxy, game, cancellationToken );//pass message
					else if( ( gameStarted = await proxy.TryReceiveAsync<GameStarted>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for GameStarted
						await GameStartedAsync( proxy, gameStarted, cancellationToken );//process request
					else if( ( data = await proxy.TryReceiveAsync<Data>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for Data
						await PassPlayerMessageAsync( proxy, data, cancellationToken );//pass message
					else if( ( knowledgeExchangeRequest = await proxy.TryReceiveAsync<KnowledgeExchangeRequest>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for KnowledgeExchangeRequest
						await PassPlayerMessageAsync( proxy, knowledgeExchangeRequest, cancellationToken );//pass message
					else if( ( rejectKnowledgeExchange = await proxy.TryReceiveAsync<RejectKnowledgeExchange>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RejectKnowledgeExchange
						await PassPlayerMessageAsync( proxy, rejectKnowledgeExchange, cancellationToken );//pass message
					else if( ( suggestAction = await proxy.TryReceiveAsync<SuggestAction>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for SuggestAction
						await PassPlayerMessageAsync( proxy, suggestAction, cancellationToken );//pass message
					else if( ( suggestActionResponse = await proxy.TryReceiveAsync<SuggestActionResponse>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for SuggestActionResponse
						await PassPlayerMessageAsync( proxy, suggestActionResponse, cancellationToken );//pass message
					else//doesn't matter
						proxy.Discard();
				}
			}
			catch( Exception )//disconnect GameMaster
			{
				var gameMasterDisconnected = new GameMasterDisconnected
				{
					GameId = proxy.Remote.Id
				};
				gamesById.TryGetValue( proxy.Remote.Id, out var game );
				if( game.GameInfo is null )
					foreach( var id in game.Players )
					{
						if( players.TryGetValue( id.Key, out var session ) )
						{
							session.GameId = Constants.AnonymousId;
							try
							{
								await session.Player.SendAsync( gameMasterDisconnected, cancellationToken );
							}
							catch( Exception )//Player fault
							{
							}
						}
						//else Player fault
					}
				throw;
			}
		}
		protected async Task PassPlayerMessageAsync< T >( IClientProxy proxy, T playerMessage, CancellationToken cancellationToken ) where T : PlayerMessage//pass to registered Player
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( players.TryGetValue( playerMessage.PlayerId, out var session ) )//if player exists
				try
				{
					await session.Player.SendAsync( playerMessage, cancellationToken );
				}
				catch( Exception )//Player fault
				{
				}
			//else GameMaster fault
		}
		protected async Task ConfirmJoiningGameAsync( IClientProxy proxy, ConfirmJoiningGame confirmJoiningGame, CancellationToken cancellationToken )//when ConfirmJoiningGame is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( players.TryGetValue( confirmJoiningGame.PlayerId, out var session ) )//if player exists
			{
				if( session.GameId is Constants.AnonymousId )
				{
					session.GameId = confirmJoiningGame.GameId;
					gamesById.TryGetValue( proxy.Remote.Id, out var game );
					game.Players.TryAdd( proxy.Remote.Id, proxy.Remote.Id );
					try
					{
						await session.Player.SendAsync( confirmJoiningGame, cancellationToken );
					}
					catch( Exception )//Player fault
					{
					}
				}
				//else Player fault
			}
			//else GameMaster fault
		}
		protected Task GameStartedAsync( IClientProxy proxy, GameStarted gameStarted, CancellationToken cancellationToken )//when GameStarted is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			gamesById.TryGetValue( proxy.Remote.Id, out var game );
			game.GameInfo = null;
			return Task.CompletedTask;
		}
		#endregion
	}
}
