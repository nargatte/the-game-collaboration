using GameMasterCore.Base.GameMasters;
using Shared.DTOs.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.GameMasters
{
    public class GameMaster : GameMasterBase
    {
        #region GameMasterBase
        public override Task RunAsync( CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }
        #endregion
        #region GameMaster
        public GameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval ) : base( gameDefinition, actionCosts, retryRegisterGameInterval )
        {
        }
        #endregion
    }
}
