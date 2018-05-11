using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ClientProxyBase : ProxyBase, IClientProxy
	{
		#region ClientProxyBase
		protected ClientProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval, cancellationToken )
		{
		}
		#endregion
	}
}
