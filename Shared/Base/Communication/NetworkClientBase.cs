using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkClientBase : NetworkComponentBase, INetworkClient
	{
		#region NetworkComponentBase
		public override void Dispose()
		{
			Client.Close();
			Client.Dispose();
		}
		#endregion
		#region INetworkClient
		public abstract Task SendAsync( string message, CancellationToken cancellationToken );
		public abstract Task<string> ReceiveAsync( CancellationToken cancellationToken );
		#endregion
		#region NetworkClientBase
		protected TcpClient Client { get; }
		protected NetworkClientBase( TcpClient client, INetworkFactory factory ) : base( factory ) => Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
		#endregion
	}
}
