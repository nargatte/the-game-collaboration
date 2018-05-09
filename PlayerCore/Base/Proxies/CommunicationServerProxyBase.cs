using PlayerCore.Interfaces.Proxies;
using Shared.Components.Extensions;
using Shared.Components.Serialization;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Base.Proxies
{
	public class CommunicationServerProxyBase : ICommunicationServerProxy
	{
		#region ICommunicationServerProxy
		public virtual void Dispose() => Client.Dispose();
		public virtual string Ip { get; }
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual INetworkFactory Factory { get; }
		public virtual async Task SendAsync<T>( T message, CancellationToken cancellationToken )
		{
			string tmp = Serializer.Serialize( message );
			Console.WriteLine( $"Send to server: { tmp }" );
			await Client.SendAsync( tmp, cancellationToken );
		}
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
			if( Client is null )
				throw new NotImplementedException( nameof( Factory ) );
		}
		#endregion
	}
}
