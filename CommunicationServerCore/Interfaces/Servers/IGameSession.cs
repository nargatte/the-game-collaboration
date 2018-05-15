using Shared.DTOs.Communication;
using Shared.Interfaces.Proxies;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface IGameSession
	{
		string Name { get; }
		GameInfo GameInfo { get; set; }
		IClientProxy GameMaster { get; }
	}
}
