using System;
using System.Threading.Tasks;

namespace SingleGame.ViewModels.Commands
{
	class AsyncCommand : AsyncCommandBase
	{
		private readonly Func<Task> command;
		public AsyncCommand( Func<Task> commandArg ) => command = commandArg;
		public override bool CanExecute( object parameter ) => true;
		public override Task ExecuteAsync( object parameter ) => command();
	}
}
