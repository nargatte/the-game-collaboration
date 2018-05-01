using CommunicationServerCore.Interfaces.Factories;
using CommunicationServerCore.Interfaces.Servers;
using Shared.DTOs.Configuration; 
using Shared.Interfaces.Modules;

namespace CommunicationServerCore.Interfaces.Modules
{
	public interface ICommunicationServerModule : IModule
	{
		CommunicationServerSettings Configuration { get; }
		ICommunicationServerFactory Factory { get; }
		ICommunicationServer CommunicationServer { get; }
	}
}
