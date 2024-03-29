﻿using Shared.Base.Proxies;
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
			try
			{
				lock( keepAlive )
				{
					keepAlive.Stop();
				}
			}
			finally
			{
				base.Dispose();
			}
		}
		protected override async Task WhenKeepAliveSent( CancellationToken cancellationToken )
		{
			lock( keepAlive )
			{
				keepAlive.Postpone();
			}
			await base.WhenKeepAliveSent( cancellationToken );
		}
		#endregion
		#region ServerProxy
		private ITaskManager keepAlive;
		public ServerProxy( INetworkClient client, uint keepAliveInterval, CancellationToken cancellationToken, IIdentity local, IProxyFactory factory ) : base( client, keepAliveInterval, cancellationToken, local, factory )
		{
			keepAlive = Factory.CreateTaskManager( SendKeepAlive, ( uint )( KeepAliveInterval / Constants.KeepAliveFrequency ), true, CancellationToken );
			keepAlive.Start();
		}
		protected async Task SendKeepAlive( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Client.SendAsync( string.Empty, cancellationToken ).ConfigureAwait( false );
			OnSentKeepAlive( Local, Remote );
		}
		#endregion
	}
}
