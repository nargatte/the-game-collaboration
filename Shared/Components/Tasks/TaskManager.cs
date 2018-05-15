using Shared.Interfaces.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Tasks
{
	public class TaskManager : ITaskManager
	{
		#region ITaskManager
		public virtual Func<CancellationToken, Task> Callback { get; }
		public virtual uint Delay { get; }
		public virtual bool Repeat { get; }
		public virtual CancellationToken CancellationToken { get; }
		public virtual void Start()
		{
			if( cts is null )
			{
				cts = CancellationTokenSource.CreateLinkedTokenSource( CancellationToken );
				task = Handler( cts.Token );
			}
		}
		public virtual void Stop()
		{
			if( cts != null )
			{
				try
				{
					cts.Cancel();
					task.Wait( CancellationToken );
				}
				catch( OperationCanceledException )
				{
				}
				catch( AggregateException )
				{
					if( !task.IsCanceled )
						throw;
				}
				finally
				{
					cts.Dispose();
					cts = null;
				}
			}
		}
		public virtual void Postpone()
		{
			Stop();
			Start();
		}
		#endregion
		#region TaskManager
		private CancellationTokenSource cts;
		private Task task;
		public TaskManager( Func<CancellationToken, Task> callback, uint delay, bool repeat, CancellationToken cancellationToken )
		{
			Callback = callback;
			Delay = delay;
			Repeat = repeat;
			CancellationToken = cancellationToken;
		}
		protected async Task Handler( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			do
			{
				await Task.Delay( TimeSpan.FromMilliseconds( Delay ), cancellationToken ).ConfigureAwait( false );
				await Callback( cancellationToken ).ConfigureAwait( false );
			}
			while( Repeat );
		}
		#endregion
	}
}
