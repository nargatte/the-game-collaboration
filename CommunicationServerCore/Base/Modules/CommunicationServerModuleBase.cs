using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Modules;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;

namespace CommunicationServerCore.Base.Modules
{
	public abstract class CommunicationServerModuleBase : Module, ICommunicationServerModule
	{
		#region ICommunicationServerModule
		public virtual CommunicationServerSettings Configuration { get; }
		public virtual ICommunicationServerFactory Factory { get; }
		public virtual ICommunicationServer CommunicationServer { get; }
		#endregion
		#region BaseCommunicationServerModule
		public CommunicationServerModuleBase( int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( port )
		{
			Configuration = configuration;
			Factory = factory;
			CommunicationServer = Factory.CreateCommunicationServer( Port, Configuration.KeepAliveInterval, Factory );
		}
		#endregion
	}
}
