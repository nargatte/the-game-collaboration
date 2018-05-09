using Shared.Interfaces;
using Shared.Interfaces.Proxies;

namespace PlayerCore.Interfaces.Players
{
    public interface IPlayer : IRunnable
    {
		uint RetryJoinGameInterval { get; }
		IServerProxy Proxy { get; set; }
    }
}