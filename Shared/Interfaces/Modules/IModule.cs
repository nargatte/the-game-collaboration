using Shared.Interfaces.Events;
using Shared.Interfaces.Tasks;

namespace Shared.Interfaces.Modules
{
	public interface IModule : IRunnable, ICommunicationObserver
	{
		int Port { get; }
	}
}