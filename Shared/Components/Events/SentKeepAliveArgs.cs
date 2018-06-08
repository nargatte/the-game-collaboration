using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Events
{
	public class SentKeepAliveArgs : EventArgs
	{
		public IIdentity Local { get; }
		public IIdentity Remote { get; }
		public SentKeepAliveArgs( IIdentity local, IIdentity remote )
		{
			Local = local;
			Remote = remote;
		}
	}
}
