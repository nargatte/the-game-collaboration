using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Events
{
	public class DisconnectedArgs : EventArgs
	{
		public IIdentity Local { get; }
		public IIdentity Remote { get; }
		public DisconnectedArgs( IIdentity local, IIdentity remote )
		{
			Local = local;
			Remote = remote;
		}
	}
}
