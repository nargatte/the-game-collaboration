using Shared.Components;
using Shared.Components.Serialization;
using Shared.Components.Tasks;
using Shared.DTOs.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Proxies
{
	public abstract class ServerProxyBase : ProxyBase, IServerProxy
	{
		#region ProxyBase
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
		#region IServerProxy
		#endregion
		#region ServerProxyBase
		private TaskDelayer keepAlive;
		protected ServerProxyBase( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken ) : base( client, keepAliveInterval ) => keepAlive = new TaskDelayer( SendKeepAlive, ( int )( KeepAliveInterval / ConstHelper.KeepAliveFrequency ), cancellationToken );
		protected async Task SendKeepAlive( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "Auto keep alive sent to server." );
			await Client.SendAsync( string.Empty, cancellationToken );
		}
		#endregion
	}
}
