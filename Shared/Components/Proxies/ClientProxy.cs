using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ClientProxy : ClientProxyBase
	{
		#region ClientProxyBase
		public override void Dispose()
		{
			disconnection.Stop();
			base.Dispose();
		}
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "SERVER sent keep alive." );
			return Task.CompletedTask;
		}
		protected override async Task OnKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "SERVER received keep alive." );
			lock( disconnection )
			{
				disconnection.Postpone();
			}
			System.Console.WriteLine( "SERVER sent keep alive." );
			await Client.SendAsync( string.Empty, cancellationToken ).ConfigureAwait( false );
		}
		#endregion
		#region ClientProxy
		private ITaskManager disconnection;
		public ClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, ITaskManagerFactory factory ) : base( client, keepAliveInterval, cancellationToken, factory )
		{
			disconnection = Factory.CreateTaskManager( CheckDisconnection, KeepAliveInterval, false, CancellationToken );
			disconnection.Start();
		}
		protected Task CheckDisconnection( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "CLIENT DISCONNECTED." );
			return Task.CompletedTask;
		}
		#endregion
	}
}