using System;
using System.Threading.Tasks;

namespace TheGame.ViewModels.Commands
{
	class AsyncCommand : AsyncCommandBase
	{
		#region AsyncCommandBase
		public override bool CanExecute( object parameter ) => true;
		public override Task ExecuteAsync( object parameter ) => command();
		#endregion
		#region AsyncCommand
		private readonly Func<Task> command;
		public AsyncCommand( Func<Task> aCommand ) => command = aCommand;
		#endregion
	}
}
