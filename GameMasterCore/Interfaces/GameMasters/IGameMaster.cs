using GameMasterCore.Interfaces.Proxies;
using Shared.Interfaces;

namespace GameMasterCore.Interfaces.GameMasters
{
    public interface IGameMaster : IRunnable
    {
        ICommunicationServerProxy Proxy { get; set; }
    }
}