using CommunicationServerCore.Interfaces.Servers;
using Shared.Base.Events;
using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Base.Servers
{
	public abstract class CommunicationServerBase : CommunicationObserverBase, ICommunicationServer
	{
		#region ICommunicationServer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual string Ip { get; }
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual IProxyFactory Factory { get; }
		#endregion
		#region CommunicationServerBase
		protected CommunicationServerBase( string ip, int port, uint keepAliveInterval, IProxyFactory factory )
		{
			Ip = ip;
			Port = port;
			KeepAliveInterval = keepAliveInterval;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
		}
		#endregion
	}
}
