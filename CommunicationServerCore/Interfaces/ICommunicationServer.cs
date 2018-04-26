using System.Collections.Generic;

namespace CommunicationServerCore.Interfaces
{
    public interface ICommunicationServer
    {
		IEnumerable<IGameMasterProxy> GameMasterProxies { get; }
		IEnumerable<IPlayerProxy> PlayerProxies { get; }
	}
}