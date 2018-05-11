using CommunicationServerCore.Base.Servers;
using Shared.Components.Extensions;
using Shared.DTOs.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.IO;
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
			cancellationToken.ThrowIfCancellationRequested();
		}
		#endregion
		#region CommunicationServer
		public CommunicationServer( string ip, int port, uint keepAliveInterval, IProxyFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected async Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )
		{
			using( var proxy = Factory.CreateClientProxy( client, KeepAliveInterval, cancellationToken ) )
			{
				cancellationToken.ThrowIfCancellationRequested();
				try
				{
					GetGames getGames;
					RegisterGame registerGame;
					while( true )
					{
						cancellationToken.ThrowIfCancellationRequested();
						if( ( getGames = await proxy.TryReceiveAsync<GetGames>( cancellationToken ).ConfigureAwait( false ) ) != null )
						{
							Console.WriteLine( $"SERVER receives: { Shared.Components.Serialization.Serializer.Serialize( getGames ) }." );
							continue;//break;
						}
						else if( ( registerGame = await proxy.TryReceiveAsync<RegisterGame>( cancellationToken ).ConfigureAwait( false ) ) != null )
						{
							Console.WriteLine( $"SERVER receives: { Shared.Components.Serialization.Serializer.Serialize( registerGame ) }." );
							continue;//break;
						}
						proxy.Discard();
					}
				}
				catch( IOException )
				{
					throw;
				}
			}
		}
		#endregion
	}
}
