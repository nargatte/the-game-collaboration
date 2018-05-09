using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;

namespace Shared.Base.Proxies
{
	public abstract class ClientProxyBase : ProxyBase, IClientProxy
	{
		#region ClientProxyBase
		protected ClientProxyBase( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}
