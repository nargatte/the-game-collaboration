using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Events
{
	public class ReceivedKeepAliveArgs : EventArgs
	{
		public IIdentity Local { get; }
		public IIdentity Remote { get; }
		public object Message { get; }
		public string SerializedMessage { get; }
		public ReceivedKeepAliveArgs( IIdentity local, IIdentity remote )
		{
			Local = local;
			Remote = remote;
		}
	}
}
