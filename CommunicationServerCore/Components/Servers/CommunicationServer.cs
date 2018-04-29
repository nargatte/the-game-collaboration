using System.Collections.Generic;
using CommunicationServerCore.Interfaces;
using Shared.DTOs.Configuration;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : ICommunicationServer
	{
		#region ICommunicationServer
		public virtual void Start()
		{

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
		#endregion
	}
}
