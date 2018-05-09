using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Interfaces.Proxies
{
	public interface ICommunicationServerProxy : IDisposable
	{
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
		Task SendAsync< T >( T message, CancellationToken cancellationToken );
	}
}
