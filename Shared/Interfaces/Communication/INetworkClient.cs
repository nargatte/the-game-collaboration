using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Communication
{
	public interface INetworkClient : INetworkComponent
	{
		Task SendAsync( string message, CancellationToken cancellationToken );
		Task<string> ReceiveAsync( CancellationToken cancellationToken );
	}
}
