using Shared.Components.Serialization;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Proxies
{
	public abstract class ProxyBase : IProxy
	{
		#region IProxy
		public virtual void Dispose() => Client.Dispose();
		public virtual uint KeepAliveInterval { get; }
		public virtual async Task SendAsync<T>( T message, CancellationToken cancellationToken ) where T : class
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Client.SendAsync( Serializer.Serialize( message ), cancellationToken ).ConfigureAwait( false );
		}
		public virtual async Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class
		{
			cancellationToken.ThrowIfCancellationRequested();
			if( buffer is null )
				buffer = await Client.ReceiveAsync( cancellationToken ).ConfigureAwait( false );
			var message = Serializer.Deserialize< T >( buffer );
			if( message != null )
				Discard();
			return message;
		}
		public virtual void Discard() => buffer = null;
		#endregion
		#region ProxyBase
		protected INetworkClient Client { get; }
		private string buffer;
		protected ProxyBase( INetworkClient client, uint keepAliveInterval )
		{
			Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
			KeepAliveInterval = keepAliveInterval;
		}
		#endregion
	}
}
