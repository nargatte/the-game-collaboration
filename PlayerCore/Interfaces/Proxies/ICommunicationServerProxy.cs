using Shared.Interfaces.Factories;
using System;

namespace PlayerCore.Interfaces.Proxies
{
	public interface ICommunicationServerProxy : IDisposable
	{
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
	}
}
