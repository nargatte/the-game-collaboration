using Shared.Interfaces.Factories;

namespace Shared.Interfaces.Proxies
{
	interface IClientProxy : IProxy
	{
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
	}
}
