using TheGame.Models;
using TheGame.ViewModels.Base;

namespace TheGame.ViewModels
{
	interface IMainVM : IViewModel
	{
	}
	class MainVM : ViewModel, IMainVM
	{
		#region ViewModel
		#endregion
		#region IMainVM
		#endregion
		#region MainVM
		public MainVM() : base( new MainM() )
		{
		}
		#endregion
	}
}
