using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TheGame.ViewModels.Commands
{
	interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync( object parameter );
	}
	abstract class AsyncCommandBase : IAsyncCommand
	{
		#region IAsyncCommand
		public abstract bool CanExecute( object parameter );
		public virtual async void Execute( object parameter ) => await ExecuteAsync( parameter );
		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}
		public abstract Task ExecuteAsync( object parameter );
		#endregion
		#region AsyncCommandBase
		protected void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
		#endregion
	}
}
