using Shared.Components.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Net.Sockets;

namespace Shared.Components.Factories
{
	public class NetworkFactory : INetworkFactory
	{
		#region INetworkFactory
		public virtual INetworkClient CreateNetworkClient( TcpClient client ) => new NetworkClient( client );
		public virtual INetworkServer CreateNetworkServer( TcpListener listener ) => new NetworkServer( listener, this );
		#endregion
	}
}
