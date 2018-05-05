using PlayerCore.Interfaces.Proxies;
using Shared.Interfaces;

namespace PlayerCore.Interfaces.Players
{
    public interface IPlayer : IRunnable
    {
		uint RetryJoinGameInterval { get; }
		ICommunicationServerProxy Proxy { get; set; }
    }
}