using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, ITaskManagerFactory factory ) : base( client, keepAliveInterval, cancellationToken, factory )
		{
		}
		#endregion
	}
}
