using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Communication
{
	public interface INetworkServer : INetworkComponent
	{
		Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken );
	}
}
