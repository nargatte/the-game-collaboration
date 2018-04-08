using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Components.Boards;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IBoardVM : IViewModel
	{
		int Width { get; }
		int Height { get; }
		ObservableCollection<IFieldVM> Fields { get; }
	}
	class BoardVM : ViewModel, IBoardVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
			for( int j = ( int )board.Height - 1; j >= 0; --j )
				for( int i = 0; i < board.Width; ++i )
				{
					Fields.Add( fields[ i, j ] = new FieldVM() );
				}
		}
		#endregion
		#region IBoardVM
		public virtual int Width { get; }
		public virtual int Height { get; }
		public virtual ObservableCollection<IFieldVM> Fields { get; private set; } = new ObservableCollection<IFieldVM>();
		#endregion
		#region BoardVM
		private IReadOnlyBoard board;
		private IFieldVM[,] fields;
		public BoardVM( IReadOnlyBoard aBoard )
		{
			board = aBoard;
			Width = ( int )board.Width;
			Height = ( int )board.Height;
			fields = new IFieldVM[ Width, Height ];
		}
		#endregion
	}
}
