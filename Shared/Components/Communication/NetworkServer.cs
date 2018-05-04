using Shared.Base.Communication;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Communication
{
	public class NetworkServer : NetworkServerBase
	{
		#region NetworkServerBase
		public override void Dispose() => Listener.Stop();
		public override async Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Factory.CreateNetworkClient( await Listener.AcceptTcpClientAsync().WithCancellation( cancellationToken ).ConfigureAwait( false ) );
		}
		#endregion
		#region NetworkServer
		public NetworkServer( TcpListener listener, INetworkFactory factory ) : base( listener, factory ) => listener.Start();
		#endregion
	}
}
