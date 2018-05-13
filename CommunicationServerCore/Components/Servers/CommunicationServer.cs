using CommunicationServerCore.Base.Servers;
using Shared.Components.Extensions;
using Shared.DTOs.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System;
using System.Collections.Generic;
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
		private IDictionary<string, GameInfo> games = new Dictionary<string, GameInfo>();
		private IDictionary<ulong, IClientProxy> gameMasters = new Dictionary<ulong, IClientProxy>();
		public CommunicationServer( string ip, int port, uint keepAliveInterval, IProxyFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected async Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )
		{
			using( var proxy = Factory.CreateClientProxy( client, KeepAliveInterval, cancellationToken, Factory.MakeIdentity() ) )
			{
				cancellationToken.ThrowIfCancellationRequested();
				PassAll( proxy );
				await Task.Delay( 20000, cancellationToken );
				/*while( true )
				{
					GetGames getGames;
					RegisterGame registerGame;
					if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						await AsAnonymousPlayer( proxy, getGames, cancellationToken ).ConfigureAwait( false );
						continue;
					}
					else if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						await AsAnonymousGameMaster( proxy, registerGame, cancellationToken ).ConfigureAwait( false );
						break;
					}
					proxy.Discard();
				}*/
			}
		}
		protected Task AsAnonymousPlayer( IClientProxy proxy, GetGames getGames, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.CompletedTask;
		}
		protected async Task AsAnonymousGameMaster( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			await PerformRegisterGame( proxy, registerGame, cancellationToken );
		}
		protected async Task PerformRegisterGame( IClientProxy proxy, RegisterGame registerGame, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( registerGame.NewGameInfo?.GameName is null || games.ContainsKey( registerGame.NewGameInfo.GameName ) )
			{
				await proxy.SendAsync( new RejectGameRegistration() { GameName = registerGame.NewGameInfo?.GameName }, cancellationToken );
			}
			else
			{
				ulong id = ( ulong )Interlocked.Increment( ref nextGameId );
				//
			}
		}
		#endregion
	}
}
