using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkServerBase : INetworkServer
	{
		#region INetworkServer
		public virtual void Dispose() => Listener.Stop();
		public virtual INetworkFactory Factory { get; }
		public abstract Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken );
		#endregion
		#region NetworkServerBase
		protected TcpListener Listener { get; }
		protected NetworkServerBase( TcpListener listener, INetworkFactory factory )
		{
			Listener = listener is null ? throw new ArgumentNullException( nameof( listener ) ) : listener;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			Listener.Start();
		}
		#endregion
	}
}
