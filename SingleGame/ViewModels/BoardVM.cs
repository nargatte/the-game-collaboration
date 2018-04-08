using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Components.Boards;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IBoardVM : IViewModel
	{
		ObservableCollection<IFieldVM> Fields { get; }
	}
	class BoardVM : ViewModel, IBoardVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
		}
		#endregion
		#region IBoardVM
		public ObservableCollection<IFieldVM> Fields { get; private set; } = new ObservableCollection<IFieldVM>();
		#endregion
		#region BoardVM
		private IReadOnlyBoard board;
		public BoardVM( IReadOnlyBoard aBoard ) => board = aBoard;
		#endregion
	}
}
