using System;

namespace Shared.Components.Events
{
	public class ExitArgs : EventArgs
	{
		public Exception Exception { get; }
		public bool IsSuccess => Exception is null;
		public ExitArgs( Exception exception = null ) => Exception = exception;
	}
}
