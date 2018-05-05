using Shared.Interfaces;

namespace PlayerCore.Interfaces.Players
{
    public interface IPlayer : IRunnable
    {
		uint RetryJoinGameInterval { get; }
    }
}