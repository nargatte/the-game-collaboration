using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Tasks
{
	public class TaskDelayer : IDisposable
	{
		#region IDisposable
		public virtual void Dispose() => Stop();
		#endregion
		#region TaskDelayer
		public void Postpone()
		{
			lock( cts )
			{
				Stop();
				Start();
			}
		}
		protected Func<CancellationToken, Task> Callback { get; }
		protected int Delay { get; }
		protected CancellationToken CancellationToken { get; }
		private CancellationTokenSource cts;
		private Task task;
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
			catch( AggregateException )
			{
				if( !task.IsCanceled )
					throw;
			}
			finally
			{
				cts.Dispose();
			}
		}
		#endregion
	}
}
