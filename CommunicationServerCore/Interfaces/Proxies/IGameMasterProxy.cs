using CommunicationServerCore.Interfaces.Servers;

namespace CommunicationServerCore.Interfaces.Proxies
{
	public interface IGameMasterProxy
    {
        ICommunicationServer CommunicationServer { get; set; }
    }
}