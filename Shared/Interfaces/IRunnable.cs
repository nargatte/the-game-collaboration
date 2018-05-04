using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
	public interface IRunnable
	{
		Task RunAsync( CancellationToken cancellationToken );
	}
}
