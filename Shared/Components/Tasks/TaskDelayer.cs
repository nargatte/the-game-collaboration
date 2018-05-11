using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Tasks
{
	public class TaskDelayer : IDisposable
	{
		#region IDisposable
		public virtual void Dispose()
		{
			Stop();
			semaphore.Dispose();
		}
		#endregion
		#region TaskDelayer
		public void Postpone()
		{
			semaphore.Wait();
			try
			{
				Stop();
				Start();
			}
			finally
			{
				semaphore.Release();
			}
		}
		protected Func<CancellationToken, Task> Callback { get; }
		protected int Delay { get; }
		protected CancellationToken CancellationToken { get; }
		private CancellationTokenSource cts;
		private Task task;
		private SemaphoreSlim semaphore = new SemaphoreSlim( 1, 1 );
		public TaskDelayer( Func<CancellationToken, Task> callback, int delay, CancellationToken cancellationToken )
		{
			Callback = callback;
			Delay = delay;
			CancellationToken = cancellationToken;
			Start();
		}
		protected void Start()
		{
			cts = CancellationTokenSource.CreateLinkedTokenSource( CancellationToken );
			task = Handler( cts.Token );
		}
		protected async Task Handler( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			while( true )
			{
				await Task.Delay( Delay, cancellationToken ).ConfigureAwait( false );
				await Callback( cancellationToken ).ConfigureAwait( false );
			}
		}
		protected void Stop()
		{
			cts.Cancel();
			try
			{
				task.Wait();
			}
			catch( OperationCanceledException )
			{
			}
			catch( Exception )
			{
				//throw;
			}
			finally
			{
				cts.Dispose();
			}
		}
		#endregion
	}
}
