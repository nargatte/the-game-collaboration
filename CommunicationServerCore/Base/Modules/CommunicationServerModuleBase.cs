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
		protected CommunicationServerModuleBase( string ip, int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( ip, port )
		{
			Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			CommunicationServer = Factory.CreateCommunicationServer( Ip, Port, Configuration.KeepAliveInterval );
			if( CommunicationServer is null )
				throw new NotImplementedException( nameof( Factory ) );
			PassAll( CommunicationServer );
		}
		#endregion
	}
}
