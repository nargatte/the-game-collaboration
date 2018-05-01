using CommunicationServerCore.Components.Servers;
using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Components.Factories;
using Shared.Interfaces.Factories;

namespace CommunicationServerCore.Components.Factories
{
	public class CommunicationServerFactory : NetworkFactory, ICommunicationServerFactory
	{
		#region ICommunicationServerFactory
		public ICommunicationServer CreateCommunicationServer( int port, uint keepAliveInterval, INetworkFactory factory ) => new CommunicationServer( port, keepAliveInterval, factory );
		#endregion
	}
}
