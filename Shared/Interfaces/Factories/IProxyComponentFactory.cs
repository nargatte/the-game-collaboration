using Shared.Enums;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Factories
{
	public interface IProxyComponentFactory
	{
		IIdentity CreateIdentity( HostType type, ulong id );
		ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken );
	}
}
