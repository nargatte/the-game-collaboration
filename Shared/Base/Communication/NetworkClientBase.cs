using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Communication
{
	public abstract class NetworkClientBase : INetworkClient
	{
		#region INetworkClient
		public virtual void Dispose() => Client.Close();
		public virtual INetworkFactory Factory { get; }
		public abstract Task SendAsync( string message, CancellationToken cancellationToken );
		#endregion
		#region NetworkClientBase
		protected TcpClient Client { get; }
		protected NetworkClientBase( TcpClient client, INetworkFactory factory )
		{
			Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
		}
		#endregion
	}
}
