using CommunicationServerCore.Base.Servers;
using CommunicationServerCore.Interfaces.Proxies;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System.Collections.Generic;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : CommunicationServerBase
	{
		#region CommunicationServerBase
		public override void Start()
		{
			var server = Factory.MakeNetworkServer( Port );
			while( true )
			{
				System.Console.WriteLine( $"CommunicationServer.Start.loop on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
				server.Accept( OnAccept );
			}
		}
		public override IEnumerable<IGameMasterProxy> GameMasterProxies => throw new System.NotImplementedException();
		public override IEnumerable<IPlayerProxy> PlayerProxies => throw new System.NotImplementedException();
		#endregion
		#region CommunicationServer
		public CommunicationServer( int port, uint keepAliveInterval, INetworkFactory factory ) : base( port, keepAliveInterval, factory )
		{
		}
		protected void OnAccept( INetworkClient client ) => System.Console.WriteLine( $"CommunicationServer.OnAccept on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
		#endregion
	}
}
