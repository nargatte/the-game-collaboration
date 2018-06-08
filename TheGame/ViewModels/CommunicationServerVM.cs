using System;
using System.Threading;
using System.Threading.Tasks;
using CommunicationServerCore.Interfaces.Modules;
using Shared.Components.Exceptions;
using Shared.Interfaces.Tasks;
using TheGame.ViewModels.Base;
using TheGame.ViewModels.Commands;

namespace TheGame.ViewModels
{
	interface ICommunicationServerVM : IViewModel, IRunnable
	{
		int Id { get; }
		string Header { get; }
		ICommunicationServerModule Module { get; }
		IAsyncCommand CancelAsyncCommand { get; }
		string Error { get; }
		ICommunicationVM Communication { get; }
	}
	class CommunicationServerVM : ViewModel, ICommunicationServerVM
	{
		#region ICommunicationServerVM
		public virtual async Task RunAsync( CancellationToken cancellationToken )
		{
			try
			{
				using( var cts = CancellationTokenSource.CreateLinkedTokenSource( cancellationToken ) )
				{
					cancellationTokenSource = cts;
					cts.Token.ThrowIfCancellationRequested();
					var task = Task.Run( async () => await Module.RunAsync( cts.Token ).ConfigureAwait( false ) );
					try
					{
						await task;
					}
					catch( OperationCanceledException )
					{
					}
					finally
					{
						if( task.IsFaulted )
						{
							Error += "Module faulted:\n";
							foreach( var e in task.Exception.Flatten().InnerExceptions )
								if( e is DisconnectionException )
									Error += $"Disconnection ( { e.Message } ).\n";
								else
									Error += $"{ e.ToString() }\n";
						}
						else if( task.IsCanceled )
							Error += "Module canceled.";
						else
							Error += "Module completed.";
					}
				}
			}
			catch( Exception e )
			{
				Error += $"{ e.ToString() }\n";
			}
			finally
			{
				cancellationTokenSource = null;
			}
		}
		public virtual int Id { get; }
		public virtual string Header { get; }
		public virtual ICommunicationServerModule Module { get; }
		public virtual IAsyncCommand CancelAsyncCommand { get; }
		private string error = string.Empty;
		public virtual string Error
		{
			get => error;
			protected set => SetProperty( ref error, value );
		}
		public virtual ICommunicationVM Communication { get; }
		#endregion
		#region CommunicationServerVM
		private CancellationTokenSource cancellationTokenSource;
		public CommunicationServerVM( int id, ICommunicationServerModule module )
		{
			Id = id;
			Header = $"Server #{ id }";
			Module = module;
			CancelAsyncCommand = new AsyncCommand( Cancel );
			Communication = new CommunicationVM( Module );
		}
		protected Task Cancel()
		{
			cancellationTokenSource?.Cancel();
			return Task.CompletedTask;
		}
		#endregion
	}
}
