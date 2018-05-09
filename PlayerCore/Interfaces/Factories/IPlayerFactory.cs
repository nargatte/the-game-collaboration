using PlayerCore.Interfaces.Players;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;

namespace PlayerCore.Interfaces.Factories
{
	public interface IPlayerFactory : INetworkFactory
	{
		IPlayer CreatePlayer( uint retryJoinGameInterval );
		IServerProxy CreateProxy( INetworkClient client, uint keepAliveInterval );
	}
}
