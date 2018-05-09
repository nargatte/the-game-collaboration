using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Interfaces.Proxies
{
	public interface IClientProxy : IDisposable
	{
		Task< T > ReceiveAsync< T >( CancellationToken cancellationToken );
	}
}
