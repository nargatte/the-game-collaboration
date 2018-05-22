using PlayerCore.Components.Players;
using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;
using Shared.Components.Factories;
using Shared.Enums;

namespace PlayerCore.Components.Factories
{
	public class PlayerFactory : ProxyFactory, IPlayerFactory
	{
		#region IPlayerFactory
		public virtual IPlayer CreatePlayer( uint retryJoinGameInterval, string gameName, TeamColour team, PlayerRole role ) => new Player( retryJoinGameInterval, gameName, team, role );
		#endregion
	}
}
