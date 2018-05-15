using CommunicationServerCore.Interfaces.Servers;
using Shared.DTOs.Communication;
using Shared.Interfaces.Factories;
using Shared.Interfaces.Proxies;

namespace CommunicationServerCore.Interfaces.Factories
{
	public interface ICommunicationServerFactory : IProxyFactory
	{
		ICommunicationServer CreateCommunicationServer( string ip, int port, uint keepAliveInterval );
		IGameSession CreateGameSession( string name, GameInfo gameInfo, IClientProxy gameMaster );
		IPlayerSession CreatePlayerSession( IClientProxy player );
	}
}
