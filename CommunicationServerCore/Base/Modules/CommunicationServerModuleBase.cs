using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Modules;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;
using System;

namespace CommunicationServerCore.Base.Modules
{
	public abstract class CommunicationServerModuleBase : ModuleBase, ICommunicationServerModule
	{
		#region ICommunicationServerModule
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual ICommunicationServerFactory Factory { get; }
		public virtual ICommunicationServer CommunicationServer { get; }
		#endregion
		#region CommunicationServerModuleBase
		public CommunicationServerModuleBase( int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( port )
		{
			Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			CommunicationServer = Factory.CreateCommunicationServer( Port, Configuration.KeepAliveInterval, Factory );
		}
		#endregion
	}
}
