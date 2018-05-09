using Shared.Interfaces.Communication;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkClientBase : INetworkClient
	{
		#region INetworkClient
		public virtual void Dispose()
		{
			Client.Close();
			Client.Dispose();
		}
		public abstract Task SendAsync( string message, CancellationToken cancellationToken );
		public abstract Task<string> ReceiveAsync( CancellationToken cancellationToken );
		#endregion
		#region NetworkClientBase
		protected TcpClient Client { get; }
		protected NetworkClientBase( TcpClient client ) => Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
		#endregion
	}
}
