using Shared.Base.Communication;
using Shared.Components.Tasks;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Communication
{
	public class NetworkServer : NetworkServerBase
	{
		#region NetworkServerBase
		public override async Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			var client = Factory.CreateNetworkClient( await Listener.AcceptTcpClientAsync().WithCancellation( cancellationToken ).ConfigureAwait( false ) );
			return client is null ? throw new NotImplementedException( nameof( Factory ) ) : client;
		}
		#endregion
		#region NetworkServer
		public NetworkServer( TcpListener listener, INetworkFactory factory ) : base( listener, factory )
		{
		}
		#endregion
	}
}
