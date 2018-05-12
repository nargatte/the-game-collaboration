using Shared.Components.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Factories
{
	public class ProxyFactory : NetworkFactory, IProxyFactory
	{
		#region IProxyFactory
		public virtual IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) => new ClientProxy( client, keepAliveInterval, cancellationToken, this );
		public virtual IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) => new ServerProxy( client, keepAliveInterval, cancellationToken, this );
		public virtual ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken ) => taskManagerFactory.CreateTaskManager( callback, delay, repeat, cancellationToken );
		#endregion
		#region ProxyFactory
		private static readonly TaskManagerFactory taskManagerFactory = new TaskManagerFactory();
		#endregion
	}
}
