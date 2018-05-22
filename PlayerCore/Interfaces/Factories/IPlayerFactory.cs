using PlayerCore.Interfaces.Players;
using Shared.Enums;
using Shared.Interfaces.Factories;

namespace PlayerCore.Interfaces.Factories
{
	public interface IPlayerFactory : IProxyFactory
	{
		IPlayer CreatePlayer( uint retryJoinGameInterval, string gameName, TeamColour team, PlayerRole role );
	}
}
