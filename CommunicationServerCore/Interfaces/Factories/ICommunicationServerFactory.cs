using CommunicationServerCore.Interfaces.Servers;
using Shared.Interfaces.Factories;

namespace CommunicationServerCore.Interfaces.Factories
{
	public interface ICommunicationServerFactory : INetworkFactory
	{
		ICommunicationServer CreateCommunicationServer( string ip, int port, uint keepAliveInterval );
	}
}
