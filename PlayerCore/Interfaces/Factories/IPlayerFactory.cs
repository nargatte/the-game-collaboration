using PlayerCore.Interfaces.Players;

namespace PlayerCore.Interfaces.Factories
{
	public interface IPlayerFactory
	{
		IPlayer CreatePlayer( uint retryJoinGameInterval );
	}
}
