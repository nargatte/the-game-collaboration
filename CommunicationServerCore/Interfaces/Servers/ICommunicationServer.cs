﻿using CommunicationServerCore.Interfaces.Proxies;
using Shared.Interfaces;
using Shared.Interfaces.Factories;
using System.Collections.Generic;

namespace CommunicationServerCore.Interfaces.Servers
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