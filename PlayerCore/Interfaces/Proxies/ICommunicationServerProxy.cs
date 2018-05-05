using Shared.Interfaces.Factories;

namespace PlayerCore.Interfaces.Proxies
{
	public interface ICommunicationServerProxy
	{
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
	}
}
