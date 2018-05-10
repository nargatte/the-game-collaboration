using PlayerCore.Components.Players;
using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;
using Shared.Components.Factories;

namespace PlayerCore.Components.Factories
{
	public class PlayerFactory : ProxyFactory, IPlayerFactory
	{
		#region IPlayerFactory
		public virtual IPlayer CreatePlayer( uint retryJoinGameInterval ) => new Player( retryJoinGameInterval );
		#endregion
	}
}
