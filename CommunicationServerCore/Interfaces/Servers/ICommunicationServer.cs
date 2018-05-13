using Shared.Interfaces.Events;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Tasks;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface ICommunicationServer : IRunnable, ICommunicationObserver
    {
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		IProxyFactory Factory { get; }
	}
}