using Shared.DTOs.Configuration;
using Shared.Interfaces;
using System.Collections.Generic;

namespace CommunicationServerCore.Interfaces
{
    public interface ICommunicationServer : IStartable
    {
		int Port { get; }
		CommunicationServerSettings Configuration { get; }
		IEnumerable<IGameMasterProxy> GameMasterProxies { get; }
		IEnumerable<IPlayerProxy> PlayerProxies { get; }
	}
}