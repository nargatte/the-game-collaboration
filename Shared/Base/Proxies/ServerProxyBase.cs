using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}
