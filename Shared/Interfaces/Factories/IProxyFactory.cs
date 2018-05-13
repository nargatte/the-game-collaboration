using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Interfaces.Factories
{
	public interface IProxyFactory : IProxyComponentFactory, INetworkFactory
	{
		IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote );
		IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local );
	}
}
