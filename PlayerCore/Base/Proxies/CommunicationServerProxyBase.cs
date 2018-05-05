using PlayerCore.Interfaces.Proxies;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;

namespace PlayerCore.Base.Proxies
{
	public class CommunicationServerProxyBase : ICommunicationServerProxy
	{
		#region ICommunicationServerProxy
		public virtual string Ip { get; }
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual INetworkFactory Factory { get; }
		#endregion
		#region CommunicationServerProxyBase
		protected INetworkClient Client { get; }
		protected CommunicationServerProxyBase( string ip, int port, uint keepAliveInterval, INetworkFactory factory )
		{
			Ip = ip;
			Port = port < 0 || port > 65535 ? throw new ArgumentOutOfRangeException( nameof( port ) ) : port;
			KeepAliveInterval = keepAliveInterval;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			Client = Factory.MakeNetworkClient( Ip, Port );
		}
		#endregion
	}
}
