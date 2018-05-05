using PlayerCore.Components.Players;
using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;

namespace PlayerCore.Components.Factories
{
	public class PlayerFactory : IPlayerFactory
	{
		#region IPlayerFactory
		public virtual IPlayer CreatePlayer( uint retryJoinGameInterval ) => new Player( retryJoinGameInterval );
		#endregion
	}
}
