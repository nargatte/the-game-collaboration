using CommunicationServerCore.Interfaces.Servers;

namespace CommunicationServerCore.Interfaces.Proxies
{
	public interface IPlayerProxy
    {
		ICommunicationServer CommunicationServer { get; set; }
	}
}