using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SingleGame.Models;
using SingleGame.ViewModels.Base;

namespace SingleGame.ViewModels
{
	interface IMainVM : IViewModel
	{
		IGameMasterVM GameMaster { get; }
		int RedPlayerCount { get; }
		ObservableCollection<IPlayerVM> RedPlayers { get; }
		int BluePlayerCount { get; }
		ObservableCollection<IPlayerVM> BluePlayers { get; }
	}
	class MainVM : ViewModel, IMainVM
	{
		#region ViewModel
		protected override async Task InitializeAsync()
		{
			await base.InitializeAsync().ConfigureAwait( false );
			Model.Initialize();
			MakeVMs();
			Model.Run();
		}
		#endregion
		#region IMainVM
		private IGameMasterVM gameMaster;
		public virtual IGameMasterVM GameMaster
		{
			get => gameMaster;
			protected set => SetProperty( ref gameMaster, value );
		}
		private int redPlayerCount;
		public virtual int RedPlayerCount
		{
			get => redPlayerCount;
			protected set => SetProperty( ref redPlayerCount, value );
		}
		public virtual ObservableCollection<IPlayerVM> RedPlayers { get; private set; } = new ObservableCollection<IPlayerVM>();
		private int bluePlayerCount;
		public virtual int BluePlayerCount
		{
			get => bluePlayerCount;
			protected set => SetProperty( ref bluePlayerCount, value );
		}
		public virtual ObservableCollection<IPlayerVM> BluePlayers { get; private set; } = new ObservableCollection<IPlayerVM>();
		#endregion
		#region MainVM
		public MainVM() : base( new MainM() )
		{
		}
		protected void MakeVMs()
		{
			GameMaster = new GameMasterVM( Model.GameMaster );
			RedPlayerCount = Model.RedPlayerCount;
			for( int i = 0; i < RedPlayerCount; ++i )
				RedPlayers.Add( new PlayerVM( Model.GetRedBoard( i ), Model.GetRedId( i ) ) );
			BluePlayerCount = Model.BluePlayerCount;
			for( int i = 0; i < BluePlayerCount; ++i )
				BluePlayers.Add( new PlayerVM( Model.GetBlueBoard( i ), Model.GetBlueId( i ) ) );
		}
		#endregion
	}
}
