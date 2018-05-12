using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Factories
{
	public interface ITaskManagerFactory
	{
		ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken );
	}
}
