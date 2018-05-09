using CommunicationServerCore.Base.Servers;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
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
			finally
			{
				try
				{
					await Task.WhenAll( tasks );
				}
				catch( OperationCanceledException )
				{
				}
				catch( Exception )
				{
					throw;
				}
				finally
				{
					foreach( var task in tasks )
					{
						if( task.IsFaulted )
							Console.WriteLine( $"Server task faulted with { task.Exception }." );
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
		public CommunicationServer( string ip, int port, uint keepAliveInterval, IProxyFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )
		{
			using( var proxy = Factory.CreateClientProxy( client, KeepAliveInterval ) )
			{
				cancellationToken.ThrowIfCancellationRequested();
				return Task.CompletedTask;
			}
		}
		#endregion
	}
}
