using CommunicationServerCore.Interfaces;
using Shared.Components.Communication;
using Shared.DTOs.Configuration;
using System.Collections.Generic;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : ICommunicationServer
	{
		#region ICommunicationServer
		public virtual void Start()
		{
			var server = new NetworkServer( Port );
			while( true )
			{
				System.Console.WriteLine( $"CommunicationServer.Start.loop on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
				server.Accept( OnAccept );
			}
		}
		public virtual int Port { get; }
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual IEnumerable<IGameMasterProxy> GameMasterProxies => throw new System.NotImplementedException();
		public virtual IEnumerable<IPlayerProxy> PlayerProxies => throw new System.NotImplementedException();
		#endregion
		#region CommunicationServer
		public CommunicationServer( int port, CommunicationServerSettings configuration )
		{
			Port = port;
			Configuration = configuration;
		}
		protected void OnAccept( NetworkClient client ) => System.Console.WriteLine( $"CommunicationServer.OnAccept on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
		#endregion
	}
}
