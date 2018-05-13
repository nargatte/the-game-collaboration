using Shared.Components.Extensions;
using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region IServerProxy
		public virtual void UpdateLocal( IIdentity identity ) => Local = identity;
		#endregion
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local, IProxyComponentFactory factory ) : base( client, keepAliveInterval, cancellationToken, local, factory.MakeIdentity( HostType.CommunicationServer ), factory )
		{
		}
		#endregion
	}
}
