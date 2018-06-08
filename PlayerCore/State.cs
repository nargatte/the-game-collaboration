using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using Shared.Components.Boards;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using Shared.Components.Fields;
using Shared.Components.Factories;
using Shared.Components.Extensions;
using Shared.DTO.Communication;

namespace PlayerCore
{
    public class State
    {
        public ulong Id { get; }

        public Game Game { get; }

        public Shared.DTO.Communication.Player[] PlayersMyTeam { get; }

        public Shared.DTO.Communication.Player[] PlayersCompetitors { get; }

        public event EventHandler EndGame;

        public int LastDiscoveryCount { get; private set; } = 0;

        public IBoard Board { get; }

        public Location Location
        {
            get
            {
                IPlayer player = Board.GetPlayer(Id);
                Location location = new Location();
                location.X = (uint)player.GetX();//?? throw new NullReferenceException("Player from board has not set x property");
                location.Y = (uint)player.GetY();// ?? throw new NullReferenceException("Player from board has not set y property");
                return location;
            }
        }
        public Location LastLocalization;

        public IField Field
        {
            get => Board.GetPlayer(Id).Field ?? throw new Exception("Player Field is null");
        }

        public ulong GameId { get; }

        public string Guid { get; }

        public TeamColour TeamColour { get; private set; }

        public Shared.DTO.Communication.Piece HoldingPiece { get; set; }


        public State(Game game, ulong id, ulong gameId, string playerGuid, BoardFactory boardFactory)
        {
            GameId = gameId;
            Game = game;
            Guid = playerGuid;
            Id = id;
            Board = boardFactory.CreateBoard(Game.Board.Width, Game.Board.TasksHeight, Game.Board.GoalsHeight);
            var player = game.Players.FirstOrDefault(p => p.Id == id) ??
                throw new NullReferenceException("Player id did not found in game object");
            PlayersMyTeam = game.Players.OrderBy(q => game.PlayerId).Where(p => p.Team == player.Team).ToArray();
            PlayersCompetitors = game.Players.Where(p => p.Team != player.Team).ToArray();

            TeamColour = player.Team;

            IPlayer boardPlayer = Board.Factory.MakePlayer(id, player.Team, player.Role,
                DateTime.Now,
                Board.GetField(game.PlayerLocation));

            Board.SetPlayer(boardPlayer);
            foreach (var pl in Game.Players.Where(p => p.Id!=Id))
            {
                Board.SetPlayer(Board.Factory.MakePlayer(pl.Id, pl.Team, pl.Role));

            }
        }

        public event EventHandler<Data> ReceiveDataLog;

        public void ReceiveData(Data data)
        {
            ReceiveDataLog?.Invoke(this, data);

            var dataPiece = data.Pieces?.FirstOrDefault(p => p.PlayerIdSpecified == true && p.PlayerId == Id) ?? HoldingPiece;
            if (dataPiece != null && dataPiece.Type == PieceType.Unknown)
                dataPiece.Type = HoldingPiece?.Type ?? PieceType.Unknown;
            HoldingPiece = dataPiece;

            LastDiscoveryCount = (data.TaskFields?.Count ?? 0) > 2 ? data.TaskFields.Count : LastDiscoveryCount;
            LastLocalization = Location;

            if (data.GameFinished == true)
            {
                if (EndGame == null)
                    throw new Exception("Nobody is subscribed EndGame event");
                EndGame(this, EventArgs.Empty);
                return;
            }

            if (data.PlayerLocation != null)
            {
                Board.SetPlayerLocation(Id, data.PlayerLocation, DateTime.Now);
            }

            if(data.TaskFields != null)
                foreach (var task in data.TaskFields)
                {
                    IPlayer player = null;
                    if(task.PlayerIdSpecified == true)
                    {
                        Board.SetPlayerLocation(task.PlayerId, new Location() { X = task.X, Y = task.Y }, task.Timestamp);
                        player = Board.GetPlayer(task.PlayerId);
                    }

                    Board.SetField(Board.Factory.MakeTaskField(task.X, task.Y, task.Timestamp, player, task.DistanceToPiece));
                }

            if(data.Pieces != null)
                foreach (Shared.DTO.Communication.Piece p in data.Pieces)
                {
                    
                    var field = data.TaskFields?.FirstOrDefault(f => f.PieceIdSpecified && f.PieceId == p.Id);
                    ITaskField taskField = null;
                    if(field != null)
                        taskField = (ITaskField)Board.GetField(field.X, field.Y);

                    if (taskField != null)
                        Board.SetPiece(Board.Factory.CreateFieldPiece(p.Id, p.Type, p.Timestamp, taskField));
                }

            if(data.GoalFields != null)
                foreach (var goal in data.GoalFields)
                {
                    IPlayer player = null;
                    
                    if (goal.PlayerIdSpecified == true)
                    {
                        Board.SetPlayerLocation(goal.PlayerId, new Location() { X = goal.X, Y = goal.Y }, goal.Timestamp);
                        player = Board.GetPlayer(goal.PlayerId);
                    }

                    Location location = new Location();
                    location.X = goal.X;
                    location.Y = goal.Y;
                    GoalFieldType type = (Board.GetField(goal.X, goal.Y) as Shared.Components.Fields.GoalField).Type;
                    
                    Board.SetField(Board.Factory.CreateGoalField(goal.X, goal.Y, goal.Team, goal.Timestamp, player, goal.Type == GoalFieldType.Unknown? type: goal.Type));
                   // if (goal.type != GoalFieldType.Unknown) HoldingPiece = null;
                }
        }
    }
}
