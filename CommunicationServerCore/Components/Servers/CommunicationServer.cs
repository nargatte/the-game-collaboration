using CommunicationServerCore.Base.Servers;
using CommunicationServerCore.Interfaces.Proxies;
using Shared.Components.Extensions;
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
					System.Console.WriteLine( "ACCEPTED" );
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
		#endregion
	}
}
