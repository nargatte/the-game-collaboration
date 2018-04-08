using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Components.Boards;
using Shared.Components.Pieces;
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
			MakeFieldVMs();
			Subscribe();
			UpdateAll();
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
		protected void MakeFieldVMs()
		{
			for( int j = Height - 1; j >= 0; --j )
				for( int i = 0; i < Width; ++i )
					Fields.Add( fields[ i, j ] = new FieldVM() );
		}
		protected void Subscribe()
		{
			board.FieldChanged += ( s, e ) => UpdateField( ( int )e.X, ( int )e.Y );
			board.PieceChanged += ( s, e ) =>
			{
				var piece = board.GetPiece( e.Id );
				if( piece is IFieldPiece fieldPiece )
				{
					if( fieldPiece.Field != null )
						UpdateField( ( int )fieldPiece.Field.X, ( int )fieldPiece.Field.Y );
				}
				else if( piece is IPlayerPiece playerPiece )
				{
					if( playerPiece.Player?.Field != null )
						UpdateField( ( int )playerPiece.Player.Field.X, ( int )playerPiece.Player.Field.Y );
				}
			};
			board.PlayerChanged += ( s, e ) =>
			{
				var player = board.GetPlayer( e.Id );
				if( player.Field != null )
					UpdateField( ( int )player.Field.X, ( int )player.Field.Y );
			};
		}
		protected void UpdateField( int x, int y ) => fields[ x, y ].Update( board.GetField( ( uint )x, ( uint )y ) );
		protected void UpdateAll()
		{
			for( int i = 0; i < Width; ++i )
				for( int j = 0; j < Height; ++j )
					UpdateField( i, j );
		}
		#endregion
	}
}
