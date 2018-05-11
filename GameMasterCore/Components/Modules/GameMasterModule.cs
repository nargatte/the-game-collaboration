using GameMasterCore.Base.Modules;
using GameMasterCore.Interfaces.Factories;
using Shared.Components.Extensions;
using Shared.DTOs.Configuration;
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
			using( GameMaster.Proxy = Factory.CreateServerProxy( Factory.MakeNetworkClient( Ip, Port ), Configuration.KeepAliveInterval, cancellationToken ) )
			{
				if( GameMaster.Proxy is null )
					throw new NotImplementedException( nameof( Factory ) );
				await GameMaster.RunAsync( cancellationToken ).ConfigureAwait( false );
			}
			GameMaster.Proxy = null;
		}
        #endregion
        #region GameMasterModule
        public GameMasterModule( string ip, int port, GameMasterSettings configuration, IGameMasterFactory factory ) : base( ip, port, configuration, factory )
        {
        }
        #endregion
    }
}
