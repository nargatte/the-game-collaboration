using Shared.Interfaces.Events;
using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Proxies
{
	public interface IProxy : IDisposable, ICommunicationObserver
	{
		uint KeepAliveInterval { get; }
		Task SendAsync< T >( T message, CancellationToken cancellationToken ) where T : class;
		Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class;
		void Discard();
		CancellationToken CancellationToken { get; }
		IIdentity Local { get; }
		IIdentity Remote { get; }
		IProxyFactory Factory { get; }
	}
}
