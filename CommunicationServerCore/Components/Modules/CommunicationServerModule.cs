using CommunicationServerCore.Components.Servers;
using CommunicationServerCore.Interfaces;
using Shared.DTOs.Configuration;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : ICommunicationServerModule
	{
		#region ICommunicationServerModule
		public virtual int Port { get; }
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual ICommunicationServer CommunicationServer { get; protected set; }
		public virtual void Start()
		{
			CommunicationServer = new CommunicationServer( Port, Configuration );
			CommunicationServer.Start();
		}
		#endregion
		#region CommunicationServerModule
		public CommunicationServerModule( int port, CommunicationServerSettings configuration )
		{
			Port = port;
			Configuration = configuration;
		}
		#endregion
	}
}
