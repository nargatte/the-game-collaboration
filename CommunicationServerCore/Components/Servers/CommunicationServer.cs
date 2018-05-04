using CommunicationServerCore.Base.Servers;
using CommunicationServerCore.Interfaces.Proxies;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
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
			using( var server = Factory.MakeNetworkServer( Port ) )
			{
				while( true )
				{
					var client = await server.AcceptAsync( cancellationToken ).ConfigureAwait( false );
				}
			}
		}
		/*public override void Start()
		{
			var server = Factory.MakeNetworkServer( Port );
			while( true )
			{
				System.Console.WriteLine( $"CommunicationServer.Start.loop on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
				server.Accept( OnAccept );
			}
		}*/
		public override IEnumerable<IGameMasterProxy> GameMasterProxies => throw new System.NotImplementedException();
		public override IEnumerable<IPlayerProxy> PlayerProxies => throw new System.NotImplementedException();
		#endregion
		#region CommunicationServer
		public CommunicationServer( int port, uint keepAliveInterval, INetworkFactory factory ) : base( port, keepAliveInterval, factory )
		{
		}
		#endregion
	}
}
