using Shared.Interfaces.Events;
using Shared.Interfaces.Tasks;

namespace Shared.Interfaces.Modules
{
	public interface IModule : IRunnable, ICommunicationObserver
	{
		string Ip { get; }
		int Port { get; }
	}
}