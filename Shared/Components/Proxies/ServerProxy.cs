using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using System.Threading;

namespace Shared.Components.Proxies
{
	public class ServerProxy : ServerProxyBase
	{
		#region ServerProxy
		public ServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval, cancellationToken )
		{
		}
		#endregion
	}
}
