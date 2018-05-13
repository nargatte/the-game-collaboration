using Shared.Base.Proxies;
using Shared.Components.Events;
using Shared.Const;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;
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
			lock( keepAlive )
			{
				keepAlive.Stop();
			}
			base.Dispose();
		}
		#endregion
		#region ServerProxy
		private ITaskManager keepAlive;
		public ServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local, IProxyComponentFactory factory ) : base( client, keepAliveInterval, cancellationToken, local, factory )
		{
			keepAlive = Factory.CreateTaskManager( SendKeepAlive, ( uint )( KeepAliveInterval / ConstHelper.KeepAliveFrequency ), true, CancellationToken );
			keepAlive.Start();
			SentKeepAlive += DelayKeepAlive;
			System.Console.WriteLine( "on" );
		}
		protected void DelayKeepAlive( object s, SentKeepAliveArgs e )
		{
			CancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( "delay" );
			lock( keepAlive )
			{
				System.Console.WriteLine( "before lock" );
				keepAlive.Postpone();
				System.Console.WriteLine( "after lock" );
			}
		}
		protected async Task SendKeepAlive( CancellationToken cancellationToken )
		{
			System.Console.WriteLine( "begin" );
			cancellationToken.ThrowIfCancellationRequested();
			await Client.SendAsync( string.Empty, cancellationToken ).ConfigureAwait( false );
			OnSentKeepAlive( Local, Remote );
		}
		#endregion
	}
}
