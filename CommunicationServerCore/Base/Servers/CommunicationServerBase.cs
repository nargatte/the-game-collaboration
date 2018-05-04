using CommunicationServerCore.Interfaces.Proxies;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Base.Servers
{
	public abstract class CommunicationServerBase : ICommunicationServer
	{
		#region ICommunicationServer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual INetworkFactory Factory { get; }
		public abstract IEnumerable<IGameMasterProxy> GameMasterProxies { get; }
		public abstract IEnumerable<IPlayerProxy> PlayerProxies { get; }
		#endregion
		#region CommunicationServerBase
		protected CommunicationServerBase( int port, uint keepAliveInterval, INetworkFactory factory )
		{
			Port = port < 0 || port > 65535 ? throw new ArgumentOutOfRangeException( nameof( port ) ) : port;
			KeepAliveInterval = keepAliveInterval;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
		}
		#endregion
	}
}
