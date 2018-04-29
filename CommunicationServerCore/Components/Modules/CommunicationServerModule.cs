using CommunicationServerCore.Components.Servers;
using CommunicationServerCore.Interfaces;
using Shared.Base;
using Shared.DTOs.Configuration;
using System;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : Module, ICommunicationServerModule
	{
		#region ICommunicationServerModule
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual ICommunicationServer CommunicationServer { get; protected set; }
		public override void Start()
		{
			try
			{
				CommunicationServer = new CommunicationServer( Port, Configuration );
				CommunicationServer.Start();
				OnExit();
			}
			catch( Exception e )
			{
				OnExit( e );
			}
		}
		#endregion
		#region CommunicationServerModule
		public CommunicationServerModule( int port, CommunicationServerSettings configuration ) : base( port ) => Configuration = configuration;
		#endregion
	}
}
