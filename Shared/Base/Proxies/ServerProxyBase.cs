using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region IServerProxy
		public virtual CancellationToken CancellationToken { get; }
		#endregion
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval ) => CancellationToken = cancellationToken;
		#endregion
	}
}
