using System;
using System.Linq;
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

        Shared.Messages.Communication.Player[] PlayersMyTeam { get; }

        Shared.Messages.Communication.Player[] PlayersCompetitors { get; }

        public event EventHandler EndGame; 

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

        public IField Field
        {
            get => Board.GetPlayer(Id).Field ?? throw new Exception("Player Field is null");
        }

        public ulong GameId { get; }

        public string Guid { get; }

        public TeamColour TeamColour { get; private set; }

        public Shared.Messages.Communication.Piece HoldingPiece { get; private set; }


        public State(Game game, ulong id, ulong gameId, string playerGuid, BoardFactory boardFactory)
        {
            GameId = gameId;
            Game = game;
            Guid = playerGuid;
            Id = id;
            Board = boardFactory.CreateBoard(Game.Board.width, Game.Board.tasksHeight, Game.Board.goalsHeight);
            var player = game.Players.FirstOrDefault(p => p.id == id) ??
                throw new NullReferenceException("Player id did not found in game object");
            PlayersMyTeam = game.Players.Where(p => p.team == player.team).ToArray();
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

        public void ReceiveData(Data data)
        {
            Console.WriteLine($"Data players id {Id}");
            Console.WriteLine(data);

            HoldingPiece = data.Pieces?.FirstOrDefault(p => p.playerIdSpecified == true && p.playerId == Id);

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

            if(data.Pieces != null)
                foreach (Shared.Messages.Communication.Piece p in data.Pieces)
                {
                    var field = data.TaskFields.FirstOrDefault(f => f.pieceIdSpecified && f.pieceId == p.id);
                    ITaskField taskField = null;
                    if(field != null)
                        taskField = (ITaskField)Board.GetField(field.x, field.y);

                    if (taskField != null)
                        Board.SetPiece(Board.Factory.CreateFieldPiece(p.id, p.type, p.timestamp, taskField));
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

            if(data.GoalFields != null)
                foreach (var goal in data.GoalFields)
                {
                    IPlayer player = null;
                    if (goal.playerIdSpecified == true)
                    {
                        Board.SetPlayerLocation(goal.playerId, new Location() { x = goal.x, y = goal.y }, goal.timestamp);
                        player = Board.GetPlayer(goal.playerId);
                    }

                    Board.SetField(Board.Factory.CreateGoalField(goal.x, goal.y, goal.team, goal.timestamp, player, goal.type));
                }
        }
    }
}
