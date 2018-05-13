using Shared.Base.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
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
			lock( disconnection )
			{
				disconnection.Stop();
			}
			base.Dispose();
		}
		protected override async Task WhenKeepAliveReceived( CancellationToken cancellationToken )
		{
			await base.WhenKeepAliveReceived( cancellationToken );
			lock( disconnection )
			{
				disconnection.Postpone();
			}
			await Client.SendAsync( string.Empty, CancellationToken ).ConfigureAwait( false );
			OnSentKeepAlive( Local, Remote );
		}
		#endregion
		#region ClientProxy
		private ITaskManager disconnection;
		public ClientProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity remote, IProxyComponentFactory factory ) : base( client, keepAliveInterval, cancellationToken, remote, factory )
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