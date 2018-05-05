using Shared.Interfaces.Factories;

namespace PlayerCore.Interfaces.Proxies
{
	public interface ICommunicationServerProxy
	{
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
	}
}
