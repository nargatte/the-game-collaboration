using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
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
			MakeBoardVM();
		}
		#endregion
		#region IGameMasterVM
		private IBoardVM board;
		public virtual IBoardVM Board
		{
			get => board;
			protected set => SetProperty( ref board, value );
		}
		public virtual ObservableCollection<LogArgs> Log { get; private set; } = new ObservableCollection<LogArgs>();
		#endregion
		#region GameMasterVM
		private IGameMaster gameMaster;
		public GameMasterVM( IGameMaster aGameMaster )
		{
			gameMaster = aGameMaster;
			Initialize();
		}
		protected void Initialize() => gameMaster.Log += ( s, e ) => Application.Current.Dispatcher.Invoke( () => Log.Add( e ) );
		protected void MakeBoardVM() => Board = new BoardVM( gameMaster.Board );
		#endregion
	}
}
