using Shared.Interfaces;
using Shared.Interfaces.Factories;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface ICommunicationServer : IRunnable
    {
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
	}
}