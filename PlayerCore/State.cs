using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using Shared.Messages.Communication;
using Shared.Components.Boards;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using Shared.Components.Fields;
using Shared.Components.Factories;
using Shared.Components.Extensions;

namespace PlayerCore
{
    public class State
    {
        public ulong Id { get; }

        public Game Game { get; }

        public Shared.Messages.Communication.Player[] PlayersMyTeam { get; }

        Shared.Messages.Communication.Player[] PlayersCompetitors { get; }

        public event EventHandler EndGame;

        public int LastDiscoveryCount { get; private set; } = 0;

        public IBoard Board { get; }

        public Location Location
        {
            get
            {
                IPlayer player = Board.GetPlayer(Id);
                Location location = new Location();
                location.x = (uint)player.GetX();//?? throw new NullReferenceException("Player from board has not set x property");
                location.y = (uint)player.GetY();// ?? throw new NullReferenceException("Player from board has not set y property");
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

        public Shared.Messages.Communication.Piece HoldingPiece { get; set; }


        public State(Game game, ulong id, ulong gameId, string playerGuid, BoardFactory boardFactory)
        {
            GameId = gameId;
            Game = game;
            Guid = playerGuid;
            Id = id;
            Board = boardFactory.CreateBoard(Game.Board.width, Game.Board.tasksHeight, Game.Board.goalsHeight);
            var player = game.Players.FirstOrDefault(p => p.id == id) ??
                throw new NullReferenceException("Player id did not found in game object");
            PlayersMyTeam = game.Players.OrderBy(q => game.playerId).Where(p => p.team == player.team).ToArray();
            PlayersCompetitors = game.Players.Where(p => p.team != player.team).ToArray();

            TeamColour = player.team;

            IPlayer boardPlayer = Board.Factory.MakePlayer(id, player.team, player.type,
                DateTime.Now,
                Board.GetField(game.PlayerLocation));

            Board.SetPlayer(boardPlayer);
            foreach (var pl in Game.Players.Where(p => p.id!=Id))
            {
                Board.SetPlayer(Board.Factory.MakePlayer(pl.id, pl.team, pl.type));

            }
        }

        public event EventHandler<Data> ReceiveDataLog;

        public void ReceiveData(Data data)
        {
            ReceiveDataLog?.Invoke(this, data);

            var dataPiece = data.Pieces?.FirstOrDefault(p => p.playerIdSpecified == true && p.playerId == Id) ?? HoldingPiece;
            if (dataPiece != null && dataPiece.type == PieceType.Unknown)
                dataPiece.type = HoldingPiece?.type ?? PieceType.Unknown;
            HoldingPiece = dataPiece;

            LastDiscoveryCount = (data.TaskFields?.Length ?? 0) > 2 ? data.TaskFields.Length : LastDiscoveryCount;
            LastLocalization = Location;

            if (data.gameFinished == true)
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
                    if(task.playerIdSpecified == true)
                    {
                        Board.SetPlayerLocation(task.playerId, new Location() { x = task.x, y = task.y }, task.timestamp);
                        player = Board.GetPlayer(task.playerId);
                    }

                    Board.SetField(Board.Factory.MakeTaskField(task.x, task.y, task.timestamp, player, task.distanceToPiece));
                }

            if(data.Pieces != null)
                foreach (Shared.Messages.Communication.Piece p in data.Pieces)
                {
                    
                    var field = data.TaskFields?.FirstOrDefault(f => f.pieceIdSpecified && f.pieceId == p.id);
                    ITaskField taskField = null;
                    if(field != null)
                        taskField = (ITaskField)Board.GetField(field.x, field.y);

                    if (taskField != null)
                        Board.SetPiece(Board.Factory.CreateFieldPiece(p.id, p.type, p.timestamp, taskField));
                }

            if(data.GoalFields != null)
                foreach (var goal in data.GoalFields)
                {
                    IPlayer player = null;
                    
                    if (goal.playerIdSpecified == true)
                    {
                        Board.SetPlayerLocation(goal.playerId, new Location() { x = goal.x, y = goal.y }, goal.timestamp);
                        player = Board.GetPlayer(goal.playerId);
                    }

                    Location location = new Location();
                    location.x = goal.x;
                    location.y = goal.y;
                    GoalFieldType type = (Board.GetField(goal.x, goal.y) as Shared.Components.Fields.GoalField).Type;
                    
                    Board.SetField(Board.Factory.CreateGoalField(goal.x, goal.y, goal.team, goal.timestamp, player, goal.type == GoalFieldType.Unknown? type: goal.type));
                   // if (goal.type != GoalFieldType.Unknown) HoldingPiece = null;
                }
        }
    }
}
