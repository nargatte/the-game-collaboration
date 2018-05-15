using Shared.Interfaces.Proxies;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface IPlayerSession
	{
		IClientProxy Player { get; }
		ulong GameId { get; set; }
	}
}
