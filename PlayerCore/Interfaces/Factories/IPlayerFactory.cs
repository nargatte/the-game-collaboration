using PlayerCore.Interfaces.Players;
using PlayerCore.Interfaces.Proxies;

namespace PlayerCore.Interfaces.Factories
{
	public interface IPlayerFactory
	{
		IPlayer CreatePlayer( uint retryJoinGameInterval );
		ICommunicationServerProxy CreateProxy();
	}
}
