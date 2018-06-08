using CommunicationServerCore.Components.Servers;
using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Components.Factories;
using Shared.DTO.Communication;
using Shared.Interfaces.Proxies;

namespace CommunicationServerCore.Components.Factories
{
	public class CommunicationServerFactory : ProxyFactory, ICommunicationServerFactory
	{
		#region ICommunicationServerFactory
		public virtual ICommunicationServer CreateCommunicationServer( string ip, int port, uint keepAliveInterval ) => new CommunicationServer( ip, port, keepAliveInterval, this );
		public virtual IGameSession CreateGameSession( string name, GameInfo gameInfo, IClientProxy gameMaster ) => new GameSession( name, gameInfo, gameMaster );
		public virtual IPlayerSession CreatePlayerSession( IClientProxy player ) => new PlayerSession( player );
		#endregion
	}
}
