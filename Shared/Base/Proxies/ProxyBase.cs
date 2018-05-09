using System;
using System.Threading;
using System.Threading.Tasks;
using Shared.Components.Serialization;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;

namespace Shared.Base.Proxies
{
	public abstract class ProxyBase : IProxy
	{
		#region IProxy
		public virtual void Dispose() => Client.Dispose();
		public virtual async Task SendAsync< T >( T message, CancellationToken cancellationToken ) where T : class => await Client.SendAsync( Serializer.Serialize( message ), cancellationToken );
		public virtual Task< T > TryReceiveAsync< T >( CancellationToken cancellationToken ) where T : class => throw new System.NotImplementedException();
		public virtual void Discard() => throw new System.NotImplementedException();
		#endregion
		#region ProxyBase
		protected INetworkClient Client { get; }
		protected ProxyBase( INetworkClient client ) => Client = client is null ? throw new ArgumentNullException( nameof( client ) ) : client;
		#endregion
	}
}
