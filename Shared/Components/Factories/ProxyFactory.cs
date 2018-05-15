using Shared.Components.Proxies;
using Shared.Components.Tasks;
using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Factories
{
	public class ProxyFactory : NetworkFactory, IProxyFactory
	{
		#region IProxyFactory
		public virtual IIdentity CreateIdentity( HostType type, ulong id ) => new Identity( type, id );
		public virtual ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken ) => new TaskManager( callback, delay, repeat, cancellationToken );
		public virtual IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote ) => new ClientProxy( client, keepAliveInterval, cancellationToken, remote, this );
		public virtual IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local ) => new ServerProxy( client, keepAliveInterval, cancellationToken, local, this );
		#endregion
	}
}
