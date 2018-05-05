using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Net;
using System.Net.Sockets;

namespace Shared.Components.Extensions
{
	public static class ExtensionINetworkFactory
	{
		public static INetworkServer MakeNetworkServer( this INetworkFactory factory, int port ) => factory.CreateNetworkServer( new TcpListener( IPAddress.Parse( "127.0.0.1" ), port ) );
	}
}
