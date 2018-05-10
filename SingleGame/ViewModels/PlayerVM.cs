using Shared.Components.Boards;
using SingleGame.ViewModels.Base;
using System.Threading.Tasks;

namespace SingleGame.ViewModels
{
	interface IPlayerVM : IViewModel
	{
		int Id { get; }
		IBoardVM Board { get; }
	}
	class PlayerVM : ViewModel, IPlayerVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync().ConfigureAwait( false );
			MakeBoardVM();
		}
		#endregion
		#region IPlayerVM
		public virtual int Id { get; }
		private IBoardVM board;
		public virtual IBoardVM Board
		{
			get => board;
			protected set => SetProperty( ref board, value );
		}
		#endregion
		#region PlayerVM
		private IReadOnlyBoard boardSource;
		public PlayerVM( IReadOnlyBoard aBoardSource, int id )
		{
			boardSource = aBoardSource;
			Id = id;
		}
		protected void MakeBoardVM() => Board = new BoardVM( boardSource );
		#endregion
	}
}
