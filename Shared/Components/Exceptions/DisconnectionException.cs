using System;
using System.Runtime.Serialization;

namespace Shared.Components.Exceptions
{
	public class DisconnectionException : Exception, ISerializable
	{
		public DisconnectionException()
		{
		}
		public DisconnectionException( string message ) : base( message )
		{
		}
		public DisconnectionException( string message, Exception inner ) : base( message, inner )
		{
		}
		protected DisconnectionException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{
		}
	}
}
