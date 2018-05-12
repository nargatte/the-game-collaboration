using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Tasks
{
	public interface ITaskManager
	{
		Func<CancellationToken, Task> Callback { get; }
		uint Delay { get; }
		bool Repeat { get; }
		CancellationToken CancellationToken { get; }
		void Start();
		void Stop();
		void Postpone();
	}
}
