using System.Net.Sockets;
using Shared.Components.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;

namespace Shared.Components.Factories
{
	public class NetworkFactory : INetworkFactory
	{
		#region INetworkFactory
		public INetworkClient CreateNetworkClient( TcpClient client ) => new NetworkClient( client );
		public INetworkServer CreateNetworkServer( TcpListener listener ) => new NetworkServer( listener );
		#endregion
	}
}
