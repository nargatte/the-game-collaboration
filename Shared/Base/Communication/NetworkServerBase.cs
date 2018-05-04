using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkServerBase : INetworkServer
	{
		#region INetworkServer
		public abstract void Dispose();
		public virtual INetworkFactory Factory { get; }
		public abstract Task<INetworkClient> AcceptAsync( CancellationToken cancellationToken );
		#endregion
		#region NetworkServerBase
		protected TcpListener Listener { get; }
		protected NetworkServerBase( TcpListener listener, INetworkFactory factory )
		{
			Listener = listener;
			Factory = factory;
		}
		#endregion
	}
}
