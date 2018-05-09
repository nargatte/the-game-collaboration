﻿using Shared.Interfaces.Modules;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Modules
{
	public abstract class ModuleBase : IModule
	{
		#region IModule
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual string Ip { get; }
		public virtual int Port { get; }
		#endregion
		#region ModuleBase
		public ModuleBase( string ip, int port )
		{
			Ip = ip;
			Port = port;
		}
		#endregion
	}
}
