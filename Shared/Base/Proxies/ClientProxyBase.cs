using Shared.Components.Extensions;
using Shared.Enums;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
using System.Threading;

namespace Shared.Base.Proxies
{
	public abstract class ClientProxyBase : ProxyBase, IClientProxy
	{
		#region IClientProxy
		public virtual void UpdateRemote( IIdentity identity ) => Remote = identity;
		#endregion
		#region ClientProxyBase
		protected ClientProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote, IProxyFactory factory ) : base( client, keepAliveInterval, cancellationToken, factory.MakeIdentity( HostType.CommunicationServer ), remote, factory )
		{
		}
		#endregion
	}
}
