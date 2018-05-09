using GameMasterCore.Components.GameMasters;
using GameMasterCore.Components.Proxies;
using GameMasterCore.Interfaces.Factories;
using GameMasterCore.Interfaces.GameMasters;
using GameMasterCore.Interfaces.Proxies;
using Shared.Components.Factories;

namespace GameMasterCore.Components.Factories
{
    public class GameMasterFactory : NetworkFactory, IGameMasterFactory
    {
        #region IGameMasterFactory
        public virtual IGameMaster CreateGameMaster() => new GameMaster();
        public virtual ICommunicationServerProxy CreateProxy(string ip, int port, uint keepAliveInterval) => new CommunicationServerProxy(ip, port, keepAliveInterval, this);
        #endregion
    }
}
