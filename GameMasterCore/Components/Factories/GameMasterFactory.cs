using GameMasterCore.Components.GameMasters;
using GameMasterCore.Interfaces.Factories;
using GameMasterCore.Interfaces.GameMasters;
using Shared.Components.Factories;
using Shared.DTOs.Configuration;

namespace GameMasterCore.Components.Factories
{
	public class GameMasterFactory : ProxyFactory, IGameMasterFactory
    {
		#region IGameMasterFactory
		public virtual IGameMaster CreateGameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval ) => new GameMaster( gameDefinition, actionCosts, retryRegisterGameInterval );
        #endregion
    }
}
