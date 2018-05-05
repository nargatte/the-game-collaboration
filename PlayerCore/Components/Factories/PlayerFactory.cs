using PlayerCore.Components.Players;
using PlayerCore.Components.Proxies;
using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;
using PlayerCore.Interfaces.Proxies;
using Shared.Components.Factories;

namespace PlayerCore.Components.Factories
{
	public class PlayerFactory : NetworkFactory, IPlayerFactory
	{
		#region IPlayerFactory
		public virtual IPlayer CreatePlayer( uint retryJoinGameInterval ) => new Player( retryJoinGameInterval );
		public virtual ICommunicationServerProxy CreateProxy( int port, uint keepAliveInterval ) => new CommunicationServerProxy( port, keepAliveInterval, this );
		#endregion
	}
}
