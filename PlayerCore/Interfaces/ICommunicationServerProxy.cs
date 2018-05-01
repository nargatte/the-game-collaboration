using Shared.Interfaces;
using Shared.Interfaces.Communication;

namespace PlayerCore.Interfaces
{
    public interface ICommunicationServerProxy : INetworkClient
    {
        IPlayer Player { get; set; }
    }
}