using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Tasks
{
	public interface IRunnable
	{
		Task RunAsync( CancellationToken cancellationToken );
	}
}
