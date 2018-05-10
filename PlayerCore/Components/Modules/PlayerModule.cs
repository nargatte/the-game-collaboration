using PlayerCore.Base.Modules;
using PlayerCore.Interfaces.Factories;
using Shared.Components.Extensions;
using Shared.DTOs.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Modules
{
	public class PlayerModule : PlayerModuleBase
	{
		#region IPlayerModule
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			using( Player.Proxy = Factory.CreateServerProxy( Factory.MakeNetworkClient( Ip, Port ), Configuration.KeepAliveInterval ) )
			{
				if( Player.Proxy is null )
					throw new NotImplementedException( nameof( Factory ) );
				await Player.RunAsync( cancellationToken ).ConfigureAwait( false );
			}
			Player.Proxy = null;
		}
		#endregion
		#region PlayerModule
		public PlayerModule( string ip, int port, PlayerSettings configuration, IPlayerFactory factory ) : base( ip, port, configuration, factory )
		{
		}
		#endregion
	}
}
