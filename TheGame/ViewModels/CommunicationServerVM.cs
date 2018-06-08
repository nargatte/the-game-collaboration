using System;
using System.Threading;
using System.Threading.Tasks;
using CommunicationServerCore.Interfaces.Modules;
using Shared.Components.Exceptions;
using Shared.Interfaces.Tasks;
using TheGame.ViewModels.Base;

namespace TheGame.ViewModels
{
	interface ICommunicationServerVM : IViewModel, IRunnable
	{
		int Id { get; }
		string Header { get; }
		ICommunicationServerModule Module { get; }
		string Error { get; }
	}
	class CommunicationServerVM : ViewModel, ICommunicationServerVM
	{
		#region ViewModel
		#endregion
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
		}
		public virtual int Id { get; }
		public virtual string Header { get; }
		public virtual ICommunicationServerModule Module { get; }
		private string error;
		public virtual string Error
		{
			get => error;
			protected set => SetProperty( ref error, value );
		}
		#endregion
		#region CommunicationServerVM
		private CancellationTokenSource cancellationTokenSource;
		public CommunicationServerVM( int id, ICommunicationServerModule module )
		{
			Id = id;
			Header = $"Server #{ id }";
			Module = module;
		}
		#endregion
	}
}
