using Shared.Base.Proxies;
using Shared.Components.Tasks;
using Shared.Interfaces.Communication;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ServerProxy : ServerProxyBase
	{
		#region ServerProxyBase
		public override void Dispose()
		{
			keepAlive.Dispose();
			base.Dispose();
		}
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Keep alive sent to server." );
			keepAlive.Postpone();
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
		private TaskDelayer keepAlive;
		public ServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval, cancellationToken ) => keepAlive = new TaskDelayer( SendKeepAlive, ( int )( KeepAliveInterval / ConstHelper.KeepAliveFrequency ), CancellationToken );
		protected async Task SendKeepAlive( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Auto keep alive sent to server." );
			await Client.SendAsync( string.Empty, cancellationToken );
		}
		#endregion
	}
}
