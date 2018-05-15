using Shared.DTOs.Communication;
using Shared.Interfaces.Proxies;
using System.Collections.Concurrent;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface IGameSession
	{
		string Name { get; }
		GameInfo GameInfo { get; set; }
		IClientProxy GameMaster { get; }
		ConcurrentDictionary<ulong, ulong> Players { get; }
	}
}
