using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region ProxyBase
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive sent to server." );
			return Task.CompletedTask;
		}
		protected override Task OnKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive received from server." );
			return Task.CompletedTask;
		}
		#endregion
		#region IServerProxy
		#endregion
		#region ServerProxyBase
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}
