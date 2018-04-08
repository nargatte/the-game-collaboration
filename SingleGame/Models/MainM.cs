using CommunicationSubstitute;
using Shared.Components.Boards;

namespace SingleGame.Models
{
	interface IMainM
	{
		void Initialize();
		ulong BluePlayerCount { get; }
		ulong RedPlayerCount { get; }
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
		public virtual ulong BluePlayerCount { get; protected set; }
		public virtual ulong RedPlayerCount { get; protected set; }
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
