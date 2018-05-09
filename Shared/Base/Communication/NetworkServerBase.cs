using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkServerBase : NetworkComponentBase, INetworkServer
	{
		#region NetworkComponentBase
		public override void Dispose() => Listener.Stop();
		#endregion
		#region INetworkServer
		public abstract Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken );
		#endregion
		#region NetworkServerBase
		protected TcpListener Listener { get; }
		protected NetworkServerBase( TcpListener listener, INetworkFactory factory ) : base( factory )
		{
			Listener = listener is null ? throw new ArgumentNullException( nameof( listener ) ) : listener;
			Listener.Start();
		}
		#endregion
	}
}
