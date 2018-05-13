using Shared.Components.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System.Net.Sockets;
using System.Threading;

namespace Shared.Components.Factories
{
	public class ProxyFactory : ProxyComponentFactory, IProxyFactory
	{
		#region IProxyFactory
		public virtual INetworkClient CreateNetworkClient( TcpClient client ) => networkFactory.CreateNetworkClient( client );
		public virtual INetworkServer CreateNetworkServer( TcpListener listener ) => networkFactory.CreateNetworkServer( listener );
		public virtual IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote ) => new ClientProxy( client, keepAliveInterval, cancellationToken, remote, this );
		public virtual IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local ) => new ServerProxy( client, keepAliveInterval, cancellationToken, local, this );
		#endregion
		#region ProxyFactory
		private static readonly NetworkFactory networkFactory = new NetworkFactory();
		#endregion
	}
}
