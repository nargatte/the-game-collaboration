using CommunicationServerCore.Base.Servers;
using Shared.Components.Extensions;
using Shared.Const;
using Shared.DTOs.Communication;
using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
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
			}
			catch( Exception e )
			{
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
					if( ex != null )
						throw new AggregateException( ex );
				}
				catch( Exception e )
				{
					throw ex is null ? new AggregateException( e ) : new AggregateException( ex, e );
				}
				finally
				{
					foreach( var task in tasks )
					{
						if( task.IsFaulted )
							Console.WriteLine( $"Server task faulted with { task.Exception.GetType().Name }." );
						else if( task.IsCanceled )
							Console.WriteLine( $"Server task canceled." );
						else
							Console.WriteLine( "Server task completed." );
					}
				}
			}
		}
		#endregion
		#region CommunicationServer
		private const long nullId = 0L;
		private long nextGameId = nullId;
		private ConcurrentDictionary<string, GameInfo> games = new ConcurrentDictionary<string, GameInfo>();
		private ConcurrentDictionary<ulong, IClientProxy> gameMasters = new ConcurrentDictionary<ulong, IClientProxy>();
		public CommunicationServer( string ip, int port, uint keepAliveInterval, IProxyFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected async Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )//when new client connected
		{
			using( var proxy = Factory.CreateClientProxy( client, KeepAliveInterval, cancellationToken, Factory.MakeIdentity() ) )//make proxy for this unknown client
			{
				cancellationToken.ThrowIfCancellationRequested();
				PassAll( proxy );//pass events from proxy
				GetGames getGames = null;
				RegisterGame registerGame = null;
				while( proxy.Remote.Type is HostType.Unknown )//while cannot identify client
				{
					if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for GetGames
						proxy.UpdateRemote( Factory.MakeIdentity( HostType.Player ) );
					else if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RegisterGame
						proxy.UpdateRemote( Factory.MakeIdentity( HostType.GameMaster ) );
					else//doesn't matter
						proxy.Discard();
				}
				switch( proxy.Remote.Type )//identified client
				{
				case HostType.Player://treat as anonymous Player
					await AsAnonymousPlayer( proxy, getGames, cancellationToken ).ConfigureAwait( false );
					break;
				case HostType.GameMaster://treat as anonymous GameMaster
					await AsAnonymousGameMaster( proxy, registerGame, cancellationToken ).ConfigureAwait( false );
					break;
				}
			}
		}
		protected async Task AsAnonymousPlayer( IClientProxy proxy, GetGames getGames, CancellationToken cancellationToken )//when Player is anonymous
		{
			cancellationToken.ThrowIfCancellationRequested();
			await PerformGetGames( proxy, getGames, cancellationToken );
			while( true )
			{
				if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for GetGames
					await PerformGetGames( proxy, getGames, cancellationToken );
				else//doesn't matter
					proxy.Discard();
			}
		}
		protected async Task PerformGetGames( IClientProxy proxy, GetGames getGames, CancellationToken cancellationToken )//when GetGames is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			var registeredGames = new RegisteredGames
			{
				GameInfo = games.Values.ToArray()
			};
			await proxy.SendAsync( registeredGames, cancellationToken );
		}
		protected async Task AsAnonymousGameMaster( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )//when GameMaster is anonymous
		{
			cancellationToken.ThrowIfCancellationRequested();
			await PerformRegisterGame( proxy, registerGame, cancellationToken );//process message
			while( proxy.Remote.Id == ConstHelper.AnonymousId )//while GameMaster is anonymous
			{
				if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cancellationToken ).ConfigureAwait( false ) ) != null )//check for RegisterGame
					await PerformRegisterGame( proxy, registerGame, cancellationToken );
				else//doesn't matter
					proxy.Discard();
			}
			await AsGameMaster( proxy, cancellationToken );//continue as registered GameMaster
		}
		protected async Task PerformRegisterGame( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )//when RegisterGame is pending
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( registerGame.NewGameInfo?.GameName is null || !games.TryAdd( registerGame.NewGameInfo.GameName, registerGame.NewGameInfo ) )//if cannot register new game
			{
				var rejectGameRegistration = new RejectGameRegistration
				{
					GameName = registerGame.NewGameInfo?.GameName
				};
				await proxy.SendAsync( rejectGameRegistration, cancellationToken );
			}
			else//if registered new game
			{
				ulong id = ( ulong )Interlocked.Increment( ref nextGameId );//generate new game id
				proxy.UpdateRemote( Factory.CreateIdentity( HostType.GameMaster, id ) );//set GameMaster id
				bool _ = gameMasters.TryAdd( id, proxy );//from now GameMaster is visible to others
				var confirmGameRegistration = new ConfirmGameRegistration
				{
					GameId = id
				};
				await proxy.SendAsync( confirmGameRegistration, cancellationToken );
			}
		}
		protected Task AsGameMaster( IClientProxy proxy, CancellationToken cancellationToken )//when GameMaster is registered
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.CompletedTask;
		}
		#endregion
	}
}
