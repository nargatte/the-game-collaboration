using GameMasterCore.Interfaces.GameMasters;
using GameMasterCore.Interfaces.Proxies;
using Shared.Interfaces.Factories;

namespace GameMasterCore.Interfaces.Factories
{
    public interface IGameMasterFactory : INetworkFactory
    {
        IGameMaster CreateGameMaster();
        ICommunicationServerProxy CreateProxy(string ip, int port, uint keepAliveInterval);
    }
}
