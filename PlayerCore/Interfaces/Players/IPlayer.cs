using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;

namespace PlayerCore.Interfaces.Players
{
	public interface IPlayer : IRunnable
    {
		uint RetryJoinGameInterval { get; }
		IServerProxy Proxy { get; set; }
    }
}