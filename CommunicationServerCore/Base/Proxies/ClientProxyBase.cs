using System.Threading;
using System.Threading.Tasks;
using CommunicationServerCore.Interfaces.Proxies;
using Shared.Interfaces.Communication;

namespace CommunicationServerCore.Base.Proxies
{
	public class ClientProxyBase : IClientProxy
	{
		#region IClientProxy
		public virtual void Dispose() => Client.Dispose();
		public virtual Task<T> ReceiveAsync<T>( CancellationToken cancellationToken ) => Task.FromResult<T>( default( T ) );
		#endregion
		#region ClientProxyBase
		protected INetworkClient Client { get; }
		protected ClientProxyBase( INetworkClient client ) => Client = client;
		#endregion
	}
}
