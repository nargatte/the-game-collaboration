using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Proxies
{
	interface IProxy : IDisposable
	{
		Task SendAsync< T >( T message, CancellationToken cancellationToken ) where T : class;
		Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class;
		void Discard();
	}
}
