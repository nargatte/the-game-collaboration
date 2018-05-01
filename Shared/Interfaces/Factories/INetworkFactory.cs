using Shared.Interfaces.Communication;
using System.Net.Sockets;

namespace Shared.Interfaces.Factories
{
	public interface INetworkFactory
	{
		INetworkClient CreateNetworkClient( TcpClient client );
		INetworkServer CreateNetworkServer( TcpListener listener );
	}
}
