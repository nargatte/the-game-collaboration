using Shared.Interfaces;

namespace PlayerCore.Interfaces
{
    public interface ICommunicationServerProxy : INetworkClient
    {
        IPlayer Player { get; set; }
    }
}