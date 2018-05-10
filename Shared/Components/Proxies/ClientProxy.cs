using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ClientProxy : ClientProxyBase
	{
		#region ClientProxyBase
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive sent to client." );
			return Task.CompletedTask;
		}
		protected override Task OnKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive received from client." );
			return Task.CompletedTask;
		}
		#endregion
		#region ClientProxy
		public ClientProxy( INetworkClient client, uint keepAliveInterval ) : base( client, keepAliveInterval )
		{
		}
		#endregion
	}
}