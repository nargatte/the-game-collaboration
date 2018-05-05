using CommunicationServerCore.Base.Servers;
using CommunicationServerCore.Interfaces.Proxies;
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
				using( var server = Factory.MakeNetworkServer( Port ) )
				{
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
				catch( Exception )
				{
				}
				foreach( var task in tasks )
				{
					if( task.IsCanceled )
						Console.WriteLine( $"server task canceled" );
					else if( task.IsFaulted )
						Console.WriteLine( $"server task faulted with { task.Exception }" );
					else
						Console.WriteLine( "server task completed" );
				}
			}
		}
		public override IEnumerable<IGameMasterProxy> GameMasterProxies => throw new System.NotImplementedException();
		public override IEnumerable<IPlayerProxy> PlayerProxies => throw new System.NotImplementedException();
		#endregion
		#region CommunicationServer
		public CommunicationServer( int port, uint keepAliveInterval, INetworkFactory factory ) : base( port, keepAliveInterval, factory )
		{
		}
		protected Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			using( client )
			{

			}
			return Task.CompletedTask;
		}
		#endregion
	}
}
