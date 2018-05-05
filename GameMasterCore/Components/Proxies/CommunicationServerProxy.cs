using GameMasterCore.Base.Proxies;
using Shared.Interfaces.Factories;

namespace GameMasterCore.Components.Proxies
{
    public class CommunicationServerProxy : CommunicationServerProxyBase
    {
        #region CommunicationServerProxy
        public CommunicationServerProxy(string ip, int port, uint keepAliveInterval, INetworkFactory factory) : base(ip, port, keepAliveInterval, factory)
        {
        }
        #endregion
    }
}
