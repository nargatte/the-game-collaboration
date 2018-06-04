using Shared.Base.Events;
using Shared.Components.Serialization;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Proxies
{
	public abstract class ProxyBase : CommunicationObserverBase, IProxy
	{
		#region IProxy
		public virtual void Dispose() => Client.Dispose();
		public virtual uint KeepAliveInterval { get; }
		public virtual async Task SendAsync< T >( T message, CancellationToken cancellationToken ) where T : class
		{
			cancellationToken.ThrowIfCancellationRequested();
            string serializedMessage = Serializer.Serialize( message );
			await Client.SendAsync( serializedMessage, cancellationToken ).ConfigureAwait( false );
			OnSent( Local, Remote, message, serializedMessage );
			await WhenKeepAliveSent( cancellationToken );
		}
		public virtual async Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class
		{
			cancellationToken.ThrowIfCancellationRequested();
			while( string.IsNullOrEmpty( buffer ) )
			{
				buffer = await Client.ReceiveAsync( cancellationToken ).ConfigureAwait( false );
				await WhenKeepAliveReceived( cancellationToken );
			}
            var message = Serializer.Deserialize< T >( buffer );
            if( message != null )
			{
		        OnReceived( Local, Remote, message, buffer );
				buffer = null;
			}
			return message;
		}
		public virtual void Discard()
		{
			OnDiscarded( Local, Remote, buffer );
			buffer = null;
		}
		public virtual CancellationToken CancellationToken { get; }
		public virtual IIdentity Local { get; protected set; }
		public virtual IIdentity Remote { get; protected set; }
		public virtual IProxyFactory Factory { get; }
		#endregion
		#region ProxyBase
		protected virtual Task WhenKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			OnSentKeepAlive( Local, Remote );
			return Task.CompletedTask;
		}
		protected virtual Task WhenKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			OnReceivedKeepAlive( Local, Remote );
			return Task.CompletedTask;
		}
		protected INetworkClient Client { get; }
		private string buffer;
		protected ProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local, IIdentity remote, IProxyFactory factory )
		{
			Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
			KeepAliveInterval = keepAliveInterval;
			CancellationToken = cancellationToken;
			Local = local;
			Remote = remote;
			Factory = factory;
		}
		#endregion
	}
}
