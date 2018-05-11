using Shared.Base.Proxies;
using Shared.Components.Tasks;
using Shared.Interfaces.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ClientProxy : ClientProxyBase
	{
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive sent to client." );
			return Task.CompletedTask;
		}
		protected override async Task OnKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive received from client." );
			disconnection.Postpone();
			await Client.SendAsync( string.Empty, cancellationToken ).ConfigureAwait( false );
			System.Console.WriteLine( "Keep alive responded to client." );
		}
		#region ClientProxy
		private TaskDelayer disconnection;
		public ClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval, cancellationToken ) => disconnection = new TaskDelayer( CheckDisconnection, KeepAliveInterval, CancellationToken );
		protected Task CheckDisconnection( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Client disconnected." );
			return Task.CompletedTask;
		}
		#endregion
	}
}