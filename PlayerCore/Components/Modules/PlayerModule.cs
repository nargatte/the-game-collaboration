using PlayerCore.Base.Modules;
using PlayerCore.Interfaces.Factories;
using Shared.Components.Extensions;
using Shared.DTO.Configuration;
using Shared.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Modules
{
	public class PlayerModule : PlayerModuleBase
	{
		#region PlayerModuleBase
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				using( Player.Proxy = Factory.CreateServerProxy( Factory.MakeNetworkClient( Ip, Port ), Configuration.KeepAliveInterval, cancellationToken, Factory.MakeIdentity( HostType.Player ) ) )
				{
					if( Player.Proxy is null )
						throw new NotImplementedException( nameof( Factory ) );
					PassAll( Player.Proxy );
					await Player.RunAsync( cancellationToken ).ConfigureAwait( false );
				}
			}
			finally
			{
				Player.Proxy = null;
			}
		}
		#endregion
		#region PlayerModule
		public PlayerModule( string ip, int port, PlayerSettings configuration, string gameName, TeamColour team, PlayerRole role, IPlayerFactory factory ) : base( ip, port, configuration, gameName, team, role, factory )
		{
		}
		#endregion
	}
}
