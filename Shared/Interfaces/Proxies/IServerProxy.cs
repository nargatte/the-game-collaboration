using System.Threading;

namespace Shared.Interfaces.Proxies
{
	public interface IServerProxy : IProxy
	{
		CancellationToken CancellationToken { get; }
	}
}
