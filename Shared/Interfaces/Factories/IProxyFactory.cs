﻿using Shared.Interfaces.Communication;
using Shared.Interfaces.Proxies;

namespace Shared.Interfaces.Factories
{
	public interface IProxyFactory : INetworkFactory
	{
		IClientProxy CreateClientProxy( INetworkClient client, uint keepAliveInterval );
		IServerProxy CreateServerProxy( INetworkClient client, uint keepAliveInterval );
	}
}
