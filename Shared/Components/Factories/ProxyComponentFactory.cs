using Shared.Components.Proxies;
using Shared.Components.Tasks;
using Shared.Enums;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Factories
{
	public class ProxyComponentFactory : IProxyComponentFactory
	{
		#region ITaskManagerFactory
		public virtual IIdentity CreateIdentity( HostType type, ulong id ) => new Identity( type, id );
		public virtual ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken ) => new TaskManager( callback, delay, repeat, cancellationToken );
		#endregion
	}
}
