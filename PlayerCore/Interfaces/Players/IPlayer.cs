using Shared.Enums;
using Shared.Interfaces.Proxies;
using Shared.Interfaces.Tasks;

namespace PlayerCore.Interfaces.Players
{
	public interface IPlayer : IRunnable
    {
		uint RetryJoinGameInterval { get; }
		string GameName { get; }
		TeamColour Team { get; }
		PlayerRole Role { get; }
		IServerProxy Proxy { get; set; }
    }
}