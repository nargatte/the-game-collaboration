using Shared.Components.Events;
using System;

namespace Shared.Interfaces.Events
{
	public interface ICommunicationObserver
	{
		event EventHandler<SentArgs> Sent;
		event EventHandler<ReceivedArgs> Received;
		event EventHandler<SentKeepAliveArgs> SentKeepAlive;
		event EventHandler<ReceivedKeepAliveArgs> ReceivedKeepAlive;
		event EventHandler<DiscardedArgs> Discarded;
		event EventHandler<DisconnectedArgs> Disconnected;
	}
}
