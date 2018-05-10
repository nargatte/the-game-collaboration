using GameMasterCore.Base.GameMasters;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.GameMasters
{
	public class GameMaster : GameMasterBase
    {
        #region GameMasterBase
        public override async Task RunAsync( CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
			System.Console.WriteLine( $"GameMaster sends: { Shared.Components.Serialization.Serializer.Serialize( new RegisterGame() ) }." );
			await Proxy.SendAsync( new RegisterGame(), cancellationToken ).ConfigureAwait( false );
		}
        #endregion
        #region GameMaster
        public GameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval ) : base( gameDefinition, actionCosts, retryRegisterGameInterval )
        {
        }
        #endregion
    }
}
