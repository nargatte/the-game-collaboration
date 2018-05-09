using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Communication
{
	public interface INetworkClient : IDisposable
	{
		Task SendAsync( string message, CancellationToken cancellationToken );
		Task<string> ReceiveAsync( CancellationToken cancellationToken );
	}
}
