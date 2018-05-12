using Shared.Components.Tasks;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Factories
{
	public class TaskManagerFactory : ITaskManagerFactory
	{
		#region ITaskManagerFactory
		public virtual ITaskManager CreateTaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken ) => new TaskManager( callback, delay, repeat, cancellationToken );
		#endregion
	}
}
