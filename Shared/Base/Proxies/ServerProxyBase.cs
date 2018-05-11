using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval, cancellationToken )
		{
		}
		#endregion
	}
}
