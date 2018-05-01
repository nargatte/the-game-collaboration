using CommunicationServerCore.Interfaces.Servers;
using Shared.Interfaces.Factories;

namespace CommunicationServerCore.Interfaces.Factories
{
	public interface ICommunicationServerFactory : INetworkFactory
	{
		ICommunicationServer CreateCommunicationServer( int port, uint keepAliveInterval, INetworkFactory factory );
	}
}
