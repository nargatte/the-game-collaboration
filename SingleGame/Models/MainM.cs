using CommunicationSubstitute;
using Shared.Components.Boards;
using Shared.Interfaces;
using System.Threading;

namespace SingleGame.Models
{
    interface IMainM
    {
        //void Initialize();
        //void Run();
        //int BluePlayerCount { get; }
        //int RedPlayerCount { get; }
        //IGameMaster GameMaster { get; }
        //int GetRedId(int i);
        //IReadOnlyBoard GetRedBoard(int i);
        //int GetBlueId(int i);
        //IReadOnlyBoard GetBlueBoard(int i);
    }
    class MainM : IMainM
    {
        //	#region IMainM
        //public virtual void Initialize()
        //{
        //    //		//game = new Game();
        //    //		//game.Initialize();
        //    //		//game.RegisterPlayers();
        //    //		//game.CreatePlayers();
        //    //		//BluePlayerCount = ( int )game.GameInfo.blueTeamPlayers;
        //    //		//RedPlayerCount = ( int )game.GameInfo.redTeamPlayers;
        //}
       // public virtual void Run() => new Thread(() =>
       //{
       //    //game.StartPlayers();
       //    //game.JoinPlayers();
       //}).Start();
       // public virtual int BluePlayerCount { get; protected set; }
       // public virtual int RedPlayerCount { get; protected set; }
        //	public virtual IGameMaster GameMaster => game.GameMaster;
        //	public virtual int GetRedId( int i ) => ( int )game.RedPlayers[ i ].State.Id;
        //	public virtual IReadOnlyBoard GetRedBoard( int i ) => game.RedPlayers[ i ].State.Board;
        //	public virtual int GetBlueId( int i ) => ( int )game.BluePlayers[ i ].State.Id;
        //	public virtual IReadOnlyBoard GetBlueBoard( int i ) => game.BluePlayers[ i ].State.Board;
        //	#endregion
        //	#region MainM
        //	private Game game;
        //	public MainM()
        //	{
        //	}
        //	#endregion
    }
}
