using CommunicationSubstitute;
using Shared.Components.Boards;
using Shared.Interfaces;
using System.Threading;

namespace SingleGame.Models
{
	interface IMainM
	{
		void Initialize();
		void Run();
		ulong BluePlayerCount { get; }
		ulong RedPlayerCount { get; }
		IGameMaster GameMaster { get; }
		IReadOnlyBoard GetBlueBoard( uint i );
		IReadOnlyBoard GetRedBoard( uint i );
	}
	class MainM : IMainM
	{
		#region IMainM
		public virtual void Initialize()
		{
			game = new Game();
			game.Initialize();
			game.RegisterPlayers();
			game.CreatePlayers();
			BluePlayerCount = game.GameInfo.blueTeamPlayers;
			RedPlayerCount = game.GameInfo.redTeamPlayers;
		}
		public virtual void Run() => new Thread( () =>
		{
			game.StartPlayers();
			game.JoinPlayers();
		} ).Start();
		public virtual ulong BluePlayerCount { get; protected set; }
		public virtual ulong RedPlayerCount { get; protected set; }
		public virtual IGameMaster GameMaster => game.GameMaster;
		public virtual IReadOnlyBoard GetBlueBoard( uint i ) => game.BluePlayers[ i ].State.Board;
		public virtual IReadOnlyBoard GetRedBoard( uint i ) => game.RedPlayers[ i ].State.Board;
		#endregion
		#region MainM
		private Game game;
		public MainM()
		{
		}
		#endregion
	}
}
