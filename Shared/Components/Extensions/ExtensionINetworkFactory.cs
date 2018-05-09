using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net;
using System.Net.Sockets;

namespace Shared.Components.Extensions
{
	public static class ExtensionINetworkFactory
	{
		public static INetworkClient MakeNetworkClient( this INetworkFactory factory, string ip, int port )
		{
			var client = factory.CreateNetworkClient( new TcpClient( ip, port ) );
			return client is null ? throw new NotImplementedException( nameof( factory ) ) : client;
		}
		public static INetworkServer MakeNetworkServer( this INetworkFactory factory, string ip, int port )
		{
			var server = factory.CreateNetworkServer( new TcpListener( IPAddress.Parse( ip ), port ) );
			return server is null ? throw new NotImplementedException( nameof( factory ) ) : server;
		}
	}
}
