using Shared.DTOs.Configuration;
using Shared.Interfaces;
using Shared.Interfaces.Factories;
using System.Collections.Generic;

namespace CommunicationServerCore.Interfaces
{
    public interface ICommunicationServer : IStartable
    {
		int Port { get; }
		uint KeepAliveInterval { get; }
		INetworkFactory Factory { get; }
		IEnumerable<IGameMasterProxy> GameMasterProxies { get; }
		IEnumerable<IPlayerProxy> PlayerProxies { get; }
	}
}