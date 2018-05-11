using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Interfaces.Factories
{
	public interface IProxyFactory : INetworkFactory
	{
		IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken );
		IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken );
	}
}
