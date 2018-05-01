using CommunicationServerCore.Interfaces.Servers;
using Shared.DTOs.Configuration;
using Shared.Interfaces;

namespace CommunicationServerCore.Interfaces
{
	public interface ICommunicationServerModule : IModule
	{
		CommunicationServerSettings Configuration { get; }
		ICommunicationServer CommunicationServer { get; }
	}
}
