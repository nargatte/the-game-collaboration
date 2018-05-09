using Shared.Base.Proxies;
using Shared.Interfaces.Communication;

namespace Shared.Components.Proxies
{
	public class ClientProxy : ClientProxyBase
	{
		#region ClientProxy
		public ClientProxy( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}