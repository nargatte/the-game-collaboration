using PlayerCore.Components.Players;
using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;
using Shared.Components.Factories;
using Shared.Components.Proxies;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;

namespace PlayerCore.Components.Factories
{
	public class PlayerFactory : NetworkFactory, IPlayerFactory
	{
		#region IPlayerFactory
		public virtual IPlayer CreatePlayer( uint retryJoinGameInterval ) => new Player( retryJoinGameInterval );
		public virtual IServerProxy CreateProxy( INetworkClient client, uint keepAliveInterval ) => new ServerProxy( client, keepAliveInterval );
		#endregion
	}
}
