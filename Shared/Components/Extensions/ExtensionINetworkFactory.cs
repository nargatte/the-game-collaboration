using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Net;
using System.Net.Sockets;

namespace Shared.Components.Extensions
{
	public static class ExtensionINetworkFactory
	{
		public static INetworkClient MakeNetworkClient( this INetworkFactory factory, string ip, int port ) => factory.CreateNetworkClient( new TcpClient( ip, port ) );
		public static INetworkServer MakeNetworkServer( this INetworkFactory factory, string ip, int port ) => factory.CreateNetworkServer( new TcpListener( IPAddress.Parse( ip ), port ) );
	}
}
