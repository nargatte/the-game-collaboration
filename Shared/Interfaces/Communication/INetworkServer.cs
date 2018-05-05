using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Communication
{
	public interface INetworkServer : IDisposable
	{
		INetworkFactory Factory { get; }
		Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken );
	}
}
