using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shared.Components.Events
{
	public static class EventHelper
	{
		public static void OnEvent<T>( object sender, EventHandler<T> eventHandler, T eventArgs, [CallerMemberName] string eventName = null )
		{
			var exceptionList = new List<Exception>();
			foreach( EventHandler<T> handler in eventHandler.GetInvocationList() )
				try
				{
					handler( sender, eventArgs );
				}
				catch( Exception exception )
				{
					exceptionList.Add( exception );
				}
			if( exceptionList.Count > 0 )
				throw new AggregateException( $"Exceptions thrown by event subscribers called in { eventName }.", exceptionList );
		}
	}
}
