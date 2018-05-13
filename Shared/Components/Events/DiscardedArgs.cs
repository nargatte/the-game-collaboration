using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Events
{
	public class DiscardedArgs : EventArgs
	{
		public IIdentity Local { get; }
		public IIdentity Remote { get; }
		public string SerializedMessage { get; }
		public DiscardedArgs( IIdentity local, IIdentity remote, string serializedMessage )
		{
			Local = local;
			Remote = remote;
			SerializedMessage = serializedMessage;
		}
	}
}
