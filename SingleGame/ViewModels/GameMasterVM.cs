using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Shared.Components.Events;
using Shared.Interfaces;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IGameMasterVM : IViewModel
	{
		IBoardVM Board { get; }
		ObservableCollection<LogArgs> Log { get; }
	}
	class GameMasterVM : ViewModel, IGameMasterVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync();
			Board = new BoardVM( gameMaster.Board );
		}
		#endregion
		#region IGameMasterVM
		private IBoardVM board;
		public virtual IBoardVM Board
		{
			get => board;
			protected set => SetProperty( ref board, value );
		}
		public ObservableCollection<LogArgs> Log { get; private set; } = new ObservableCollection<LogArgs>();
		#endregion
		#region GameMasterVM
		private IGameMaster gameMaster;
		public GameMasterVM( IGameMaster aGameMaster )
		{
			gameMaster = aGameMaster;
			gameMaster.Log += ( s, e ) => Log.Add( e );
		}
		#endregion
	}
}
