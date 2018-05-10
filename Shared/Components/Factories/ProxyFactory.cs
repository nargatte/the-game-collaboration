using Shared.Components.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;

namespace Shared.Components.Factories
{
	public class ProxyFactory : NetworkFactory, IProxyFactory
	{
		public virtual IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval ) => new ClientProxy( client, keepAliveInterval );
		public virtual IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval ) => new ServerProxy( client, keepAliveInterval );
	}
}
