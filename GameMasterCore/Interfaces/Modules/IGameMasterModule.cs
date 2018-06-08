using GameMasterCore.Interfaces.Factories;
using GameMasterCore.Interfaces.GameMasters;
using Shared.DTO.Configuration;
using Shared.Interfaces.Modules;

namespace GameMasterCore.Interfaces.Modules
{
	public interface IGameMasterModule : IModule
    {
        GameMasterSettings Configuration { get; }
		IGameMasterFactory Factory { get; }
		IGameMaster GameMaster { get; }
    }
}
