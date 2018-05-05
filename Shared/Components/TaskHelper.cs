using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components
{
	public static class TaskHelper
	{
		public static async Task< T > WithCancellation<T>( this Task< T > task, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			var tcs = new TaskCompletionSource<bool>();
			using( cancellationToken.Register( s => ( s as TaskCompletionSource<bool> ).TrySetResult( true ), tcs ) )
				if( task != await Task.WhenAny( task, tcs.Task ) )
					throw new OperationCanceledException( cancellationToken );
			return await task;
		}
	}
}
