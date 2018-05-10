using Shared.Base.Proxies;
using Shared.Interfaces.Communication;

namespace Shared.Components.Proxies
{
	public class ServerProxy : ServerProxyBase
	{
		#region ServerProxyBase
		protected override void OnKeepAlive()
		{
		}
		#endregion
		#region ServerProxy
		public ServerProxy( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}
