using Shared.Interfaces.Factories;

namespace GameMasterCore.Interfaces.Proxies
{
    public interface ICommunicationServerProxy
    {
        string Ip { get; }
        int Port { get; }
        uint KeepAliveInterval { get; }
        INetworkFactory Factory { get; }
    }
}
