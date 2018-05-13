using Shared.Interfaces.Proxies;
using System;

namespace Shared.Components.Events
{
	public class SentArgs : EventArgs
	{
		public IIdentity Local { get; }
		public IIdentity Remote { get; }
		public object Message { get; }
		public string SerializedMessage { get; }
		public SentArgs( IIdentity local, IIdentity remote, object message, string serializedMessage )
		{
			Local = local;
			Remote = remote;
			Message = message;
			SerializedMessage = serializedMessage;
		}
	}
}
