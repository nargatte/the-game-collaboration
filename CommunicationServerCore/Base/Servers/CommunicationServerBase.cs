﻿using CommunicationServerCore.Interfaces.Proxies;
using CommunicationServerCore.Interfaces.Servers;
using Shared.Interfaces.Factories;
using System.Collections.Generic;

namespace CommunicationServerCore.Base.Servers
{
	public abstract class CommunicationServerBase : ICommunicationServer
	{
		#region ICommunicationServer
		public abstract void Start();
		public virtual int Port { get; }
		public virtual uint KeepAliveInterval { get; }
		public virtual INetworkFactory Factory { get; }
		public abstract IEnumerable<IGameMasterProxy> GameMasterProxies { get; }
		public abstract IEnumerable<IPlayerProxy> PlayerProxies { get; }
		#endregion
		#region CommunicationServerBase
		protected CommunicationServerBase( int port, uint keepAliveInterval, INetworkFactory factory )
		{
			Port = port;
			KeepAliveInterval = keepAliveInterval;
			Factory = factory;
		}
		#endregion
	}
}
