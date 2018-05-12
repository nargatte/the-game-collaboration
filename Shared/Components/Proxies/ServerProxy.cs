using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Proxies
{
	public class ServerProxy : ServerProxyBase
	{
		#region ServerProxyBase
		public override void Dispose()
		{
			keepAlive.Stop();
			base.Dispose();
		}
		protected override Task OnKeepAliveSent( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "CLIENT sent keep alive." );
			lock( keepAlive )
			{
				keepAlive.Postpone();
			}
			return Task.CompletedTask;
		}
		protected override Task OnKeepAliveReceived( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "CLIENT received keep alive." );
			return Task.CompletedTask;
		}
		#endregion
		#region ServerProxy
		private ITaskManager keepAlive;
		public ServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, ITaskManagerFactory factory ) : base( client, keepAliveInterval, cancellationToken, factory )
		{
			keepAlive = Factory.CreateTaskManager( SendKeepAlive, ( uint )( KeepAliveInterval / ConstHelper.KeepAliveFrequency ), true, CancellationToken );
			keepAlive.Start();
		}
		protected async Task SendKeepAlive( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "CLIENT AUTO sent keep alive." );
			await Client.SendAsync( string.Empty, cancellationToken ).ConfigureAwait( false );
		}
		#endregion
	}
}
