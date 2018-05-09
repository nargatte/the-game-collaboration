using CommunicationServerCore.Interfaces.Servers;
using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Base.Servers
{
	public abstract class CommunicationServerBase : ICommunicationServer
	{
		#region ICommunicationServer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual string Ip { get; }
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual INetworkFactory Factory { get; }
		#endregion
		#region CommunicationServerBase
		protected CommunicationServerBase( string ip, int port, uint keepAliveInterval, INetworkFactory factory )
		{
			Ip = ip;
			Port = port;
			KeepAliveInterval = keepAliveInterval;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
		}
		#endregion
	}
}
