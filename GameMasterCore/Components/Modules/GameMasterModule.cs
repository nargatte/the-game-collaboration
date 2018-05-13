using GameMasterCore.Base.Modules;
using GameMasterCore.Interfaces.Factories;
using Shared.Components.Extensions;
using Shared.DTOs.Configuration;
using Shared.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.Modules
{
    public class GameMasterModule : GameMasterModuleBase
    {
        #region GameMasterModuleBase
        public override async Task RunAsync( CancellationToken cancellationToken )
        {
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				using( GameMaster.Proxy = Factory.CreateServerProxy( Factory.MakeNetworkClient( Ip, Port ), Configuration.KeepAliveInterval, cancellationToken, Factory.MakeIdentity( HostType.GameMaster ) ) )
				{
					if( GameMaster.Proxy is null )
						throw new NotImplementedException( nameof( Factory ) );
					PassAll( GameMaster.Proxy );
					await GameMaster.RunAsync( cancellationToken ).ConfigureAwait( false );
				}
			}
			finally
			{
				GameMaster.Proxy = null;
			}
		}
        #endregion
        #region GameMasterModule
        public GameMasterModule( string ip, int port, GameMasterSettings configuration, IGameMasterFactory factory ) : base( ip, port, configuration, factory )
        {
        }
        #endregion
    }
}
