using GameMasterCore.Interfaces.GameMasters;
using Shared.DTO.Configuration;
using Shared.Interfaces.Factories;

namespace GameMasterCore.Interfaces.Factories
{
	public interface IGameMasterFactory : IProxyFactory
    {
        IGameMaster CreateGameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval );
    }
}
