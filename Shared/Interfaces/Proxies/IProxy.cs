using Shared.Components.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Proxies
{
	public interface IProxy : IDisposable
	{
		uint KeepAliveInterval { get; }
		Task SendAsync< T >( T message, CancellationToken cancellationToken ) where T : class;
		Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class;
		void Discard();
	}
}
