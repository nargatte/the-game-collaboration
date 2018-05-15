using Shared.Components.Events;
using Shared.Interfaces.Events;
using Shared.Interfaces.Proxies;
using System;

namespace Shared.Base.Events
{
	public abstract class CommunicationObserverBase : ICommunicationObserver
	{
		#region ICommunicationObserver
		public virtual event EventHandler<SentArgs> Sent = delegate { };
		public virtual event EventHandler<ReceivedArgs> Received = delegate { };
		public virtual event EventHandler<SentKeepAliveArgs> SentKeepAlive = delegate { };
		public virtual event EventHandler<ReceivedKeepAliveArgs> ReceivedKeepAlive = delegate { };
		public virtual event EventHandler<DiscardedArgs> Discarded = delegate { };
		public virtual event EventHandler<DisconnectedArgs> Disconnected = delegate { };
		#endregion
		#region CommunicationObserverBase
		protected void OnSent( IIdentity local, IIdentity remote, object message, string serializedMessage ) => PassSent( null, new SentArgs( local, remote, message, serializedMessage ) );
		protected void OnReceived( IIdentity local, IIdentity remote, object message, string serializedMessage ) => PassReceived( null, new ReceivedArgs( local, remote, message, serializedMessage ) );
		protected void OnSentKeepAlive( IIdentity local, IIdentity remote ) => PassSentKeepAlive( null, new SentKeepAliveArgs( local, remote ) );
		protected void OnReceivedKeepAlive( IIdentity local, IIdentity remote ) => PassReceivedKeepAlive( null, new ReceivedKeepAliveArgs( local, remote ) );
		protected void OnDiscarded( IIdentity local, IIdentity remote, string serializedMessage ) => PassDiscarded( null, new DiscardedArgs( local, remote, serializedMessage ) );
		protected void OnDisconnected( IIdentity local, IIdentity remote ) => PassDisconnected( null, new DisconnectedArgs( local, remote ) );
		protected void PassSent( object s, SentArgs e ) => EventHelper.OnEvent( this, Sent, e );
		protected void PassReceived( object s, ReceivedArgs e ) => EventHelper.OnEvent( this, Received, e );
		protected void PassSentKeepAlive( object s, SentKeepAliveArgs e ) => EventHelper.OnEvent( this, SentKeepAlive, e );
		protected void PassReceivedKeepAlive( object s, ReceivedKeepAliveArgs e ) => EventHelper.OnEvent( this, ReceivedKeepAlive, e );
		protected void PassDiscarded( object s, DiscardedArgs e ) => EventHelper.OnEvent( this, Discarded, e );
		protected void PassDisconnected( object s, DisconnectedArgs e ) => EventHelper.OnEvent( this, Disconnected, e );
		protected void PassAll( ICommunicationObserver communicationObserver )
		{
			communicationObserver.Sent += PassSent;
			communicationObserver.Received += PassReceived;
			communicationObserver.SentKeepAlive += PassSentKeepAlive;
			communicationObserver.ReceivedKeepAlive += PassReceivedKeepAlive;
			communicationObserver.Discarded += PassDiscarded;
			communicationObserver.Disconnected += PassDisconnected;
		}
		#endregion
	}
}
