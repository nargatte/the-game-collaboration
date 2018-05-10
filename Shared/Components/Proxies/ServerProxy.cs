using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ServerProxy : ServerProxyBase
	{
		#region ServerProxyBase
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
		#region ServerProxy
		public ServerProxy( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}
