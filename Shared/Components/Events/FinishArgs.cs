using System;

namespace Shared.Components.Events
{
	public class FinishArgs : EventArgs
	{
		public Exception Exception { get; }
		public bool IsSuccess => Exception is null;
		public FinishArgs( Exception exception = null ) => Exception = exception;
	}
}
