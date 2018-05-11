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
			Console.WriteLine( "DISPOSE" );
			Stop();
		}
		#endregion
		#region TaskDelayer
		public void Postpone()
		{
			Console.WriteLine( "POSTPONE" );
			lock( cts )
			{
				Console.WriteLine( "LOCKED" );
				Stop();
				Start();
				Console.WriteLine( "UNLOCKED" );
			}
		}
		protected Func<CancellationToken, Task> Callback { get; }
		protected uint Delay { get; }
		protected CancellationToken CancellationToken { get; }
		private CancellationTokenSource cts;
		private Task task;
		public TaskDelayer( Func<CancellationToken, Task> callback, uint delay, CancellationToken cancellationToken )
		{
			Callback = callback;
			Delay = delay;
			CancellationToken = cancellationToken;
			Start();
		}
		protected void Start()
		{
			Console.WriteLine( "START" );
			cts = CancellationTokenSource.CreateLinkedTokenSource( CancellationToken );
			task = Handler( cts.Token );
		}
		protected async Task Handler( CancellationToken cancellationToken )
		{
			Console.WriteLine( "HANDLER" );
			cancellationToken.ThrowIfCancellationRequested();
			while( true )
			{
				Console.WriteLine( "REPEAT" );
				await Task.Delay( TimeSpan.FromMilliseconds( Delay ), cancellationToken ).ConfigureAwait( false );
				await Callback( cancellationToken ).ConfigureAwait( false );
			}
		}
		protected void Stop()
		{
			Console.WriteLine( "STOP" );
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
