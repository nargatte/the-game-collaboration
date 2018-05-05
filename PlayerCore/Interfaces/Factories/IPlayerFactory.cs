using PlayerCore.Interfaces.Players;
using PlayerCore.Interfaces.Proxies;
using Shared.Interfaces.Factories;

namespace PlayerCore.Interfaces.Factories
{
	public interface IPlayerFactory : INetworkFactory
	{
		IPlayer CreatePlayer( uint retryJoinGameInterval );
		ICommunicationServerProxy CreateProxy( int port, uint keepAliveInterval );
	}
}
