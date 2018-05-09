using GameMasterCore.Interfaces.GameMasters;
using GameMasterCore.Interfaces.Proxies;
using Shared.DTOs.Configuration;
using Shared.Interfaces.Modules;

namespace GameMasterCore.Interfaces.Modules
{
    public interface IGameMasterModule : IModule
    {
        GameMasterSettings Configuration { get; }
        IGameMaster GameMaster { get; }
    }
}
