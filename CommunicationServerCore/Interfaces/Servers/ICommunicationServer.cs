﻿using Shared.Interfaces;
using Shared.Interfaces.Factories;

namespace CommunicationServerCore.Interfaces.Servers
{
	public interface ICommunicationServer : IRunnable
    {
		string Ip { get; }
		int Port { get; }
		uint KeepAliveInterval { get; }
		IProxyFactory Factory { get; }
	}
}