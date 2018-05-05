using CommunicationServerCore.Components.Servers;
using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Components.Factories;

namespace CommunicationServerCore.Components.Factories
{
	public class CommunicationServerFactory : NetworkFactory, ICommunicationServerFactory
	{
		#region ICommunicationServerFactory
		public ICommunicationServer CreateCommunicationServer( int port, uint keepAliveInterval ) => new CommunicationServer( port, keepAliveInterval, this );
		#endregion
	}
}
