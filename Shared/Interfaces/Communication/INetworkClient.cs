using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Interfaces.Communication
{
	public interface INetworkClient : IDisposable
	{
		INetworkFactory Factory { get; }
		Task SendAsync( string message, CancellationToken cancellationToken );
	}
}
