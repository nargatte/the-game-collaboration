using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Factories
{
	public interface IProxyFactory : INetworkFactory
	{
		IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote );
		IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local );
		IIdentity CreateIdentity( HostType type, ulong id );
		ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken );
	}
}
