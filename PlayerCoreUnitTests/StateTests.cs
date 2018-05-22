using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlayerCore;
using Shared.Components.Events;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.DTOs.Communication;
using Shared.Enums;

namespace PlayerCoreUnitTests
{
    [TestFixture]
    public partial class StateTests
    {
        [Test]
        public void ReceiveData_DataEmpty_NoChange()
        {
            State state = GetState(1);
            Data data = GetData();

            state.ReceiveData(data);

            Assert.Multiple(() =>
            {
                Assert.IsNull(state.HoldingPiece);
                Assert.AreEqual(1, state.Game.Players.Count);
                Assert.AreEqual(0, state.Game.PlayerLocation.X);
                Assert.AreEqual(0, state.Game.PlayerLocation.Y);
                Assert.AreEqual(10, state.Game.Board.Width);
                Assert.AreEqual(10, state.Game.Board.TasksHeight);
                Assert.AreEqual(10, state.Game.Board.GoalsHeight);
            });
        }


        [TestCase(5u, 15u)]
        [TestCase(3u, 11u)]
        [TestCase(0u, 17u)]
        public void ReceiveData_NewLocation_LocationChange(uint x, uint y)
        {
            State state = GetStateWithLocation(1, x, y);
            Data data = GetDataWithLocation(x, y);

            state.ReceiveData(data);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(x, state.Board.GetPlayer(1).Field.X);
                Assert.AreEqual(y, state.Board.GetPlayer(1).Field.Y);
            });

        }

        [TestCase(1u)]
        [TestCase(2u)]
        public void IsHoldingPiece_Test(uint id)
        {
            State state = GetState(id);
            state.Game.Players = new Shared.DTOs.Communication.Player[1];
            state.Game.Players[0] = GetPlayer(id);
            Data data = GetData();

            data.Pieces = new Shared.DTOs.Communication.Piece[1];
            var piece = GetPiece();
            piece.Id = id;
            piece.PlayerId = id;
            piece.PlayerIdSpecified = true;
            piece.Timestamp = new DateTime();
            piece.Type = PieceType.Unknown;
            data.Pieces[0] = piece;



            state.ReceiveData(data);

            Assert.AreSame(piece, state.HoldingPiece);

        }

        [Test]
        public void IsNotHoldingPiece_Test()
        {

            State state = GetState(1);
            Data data = GetData();
            state.ReceiveData(data);

            Assert.IsNull(state.HoldingPiece);
        }

        [Test]
        public void ReceiveData_EndingGame_InvokeEvent()
        {
            State state = GetState(1);
            state.Game.Players = new Shared.DTOs.Communication.Player[1];
            state.Game.Players[0] = GetPlayer(1);
            Data data = GetData();
            data.GameFinished = true;
            bool wasCalled = false;
            state.EndGame += (s, a) => wasCalled = true;

            state.ReceiveData(data);

            Assert.IsTrue(wasCalled);
        }

        [TestCase(5u, 15u)]
        [TestCase(3u,11u)]
        [TestCase(0u,17u)]
        public void ReceiveData_UpdateBoard_Taskfield_PutPlayer(uint x, uint y)
        {
            State state = GetState(1);
            Data data = GetData();

            Shared.DTOs.Communication.TaskField taskField = new Shared.DTOs.Communication.TaskField();
            taskField.X = x;
            taskField.Y = y;
            taskField.PlayerIdSpecified = true;
            taskField.PlayerId = 1;
            taskField.DistanceToPiece = 1;
            data.TaskFields = new Shared.DTOs.Communication.TaskField[1];
            data.TaskFields[0] = taskField;

            state.ReceiveData(data);

            Assert.AreEqual(taskField.PlayerId, state.Board.GetPlayer(1).Id);

        }


        [TestCase(1u, 25u)]
        [TestCase(5u,27u)]
        [TestCase(9u,28u)]
        public void ReceiveData_UpdateBoard_GoalField_PutPlayer(uint x, uint y)
        {
            State state = GetState(1);
            Data data = GetDataWithLocation(x, y);

            Shared.DTOs.Communication.GoalField goalField = new Shared.DTOs.Communication.GoalField
            {
                X = x,
                Y = y,
                PlayerIdSpecified = true,
                PlayerId = 1
            };



            data.GoalFields = new Shared.DTOs.Communication.GoalField[1];
            data.GoalFields[0] = goalField;

            state.ReceiveData(data);

            Assert.AreEqual(goalField.PlayerId, state.Board.GetPlayer(1).Id);

        }

        [TestCase(5u, 15u)]
        [TestCase(3u, 11u)]
        [TestCase(0u, 17u)]
        public void Receive_Data_Put_Piece(uint x,uint y)
        {
            State state = GetState(1);
            Data data = GetDataWithPiece(x,y);

            Shared.DTOs.Communication.Piece piece = GetPiece();

            state.ReceiveData(data);

            Assert.AreEqual(piece.Id, state.Board.GetPiece(piece.Id).Id);

        }

        private State GetState(uint id) => new State(GetGame(id), id, 0, string.Empty, new Shared.Components.Factories.BoardFactory());

        private State GetStateWithLocation(uint id, uint x, uint y) => new State(GetGameWithLocation(id,x, y), id, 0, string.Empty, new Shared.Components.Factories.BoardFactory());

        private Game GetGame(uint id)
        {
            Game game = new Game
            {
                Board = new Shared.DTOs.Communication.GameBoard
                {
                    GoalsHeight = 10,
                    TasksHeight = 10,
                    Width = 10
                },
                PlayerId = id,
                PlayerLocation = new Location(),
                
                Players = new Shared.DTOs.Communication.Player[1]
            };
            game.Players[0] = GetPlayer(id);
            
            return game;
        }

        private Game GetGameWithLocation(uint id, uint x, uint y)
        {
            Game game = new Game();
            game.Board = new Shared.DTOs.Communication.GameBoard();
            game.Board.GoalsHeight = 10;
            game.Board.TasksHeight = 10;
            game.Board.Width = 10;
            game.PlayerId = id;
            game.PlayerLocation = new Location();
            game.PlayerLocation.X = x;
            game.PlayerLocation.Y = y;
            game.Players = new Shared.DTOs.Communication.Player[1];
            game.Players[0] = GetPlayer(id);
            return game;
        }

       

        private Data GetData()
        {
            Data data = new Data();
            data.TaskFields = new Shared.DTOs.Communication.TaskField[0];
            data.GoalFields = new Shared.DTOs.Communication.GoalField[0];
            data.Pieces = new Shared.DTOs.Communication.Piece[0];
            data.PlayerLocation = new Location();
            return data;
        }

        private Data GetDataWithPiece(uint x, uint y)
        {
            Data data = new Data();
            data.TaskFields = new Shared.DTOs.Communication.TaskField[1];
            data.TaskFields[0] = new Shared.DTOs.Communication.TaskField();
            data.TaskFields[0].PieceIdSpecified = true;
            data.TaskFields[0].PieceId = 1;
            data.TaskFields[0].X = x;
            data.TaskFields[0].Y = y;

            data.GoalFields = new Shared.DTOs.Communication.GoalField[0];
            data.Pieces = new Shared.DTOs.Communication.Piece[1];
            data.Pieces[0] = GetPiece();
            data.PlayerLocation = new Location();
            data.PlayerLocation.X = x;
            data.PlayerLocation.Y = y;

            return data;
        }

        private Data GetDataWithLocation(uint x, uint y)
        {
            Data data = new Data
            {
                TaskFields = new Shared.DTOs.Communication.TaskField[0],
                GoalFields = new Shared.DTOs.Communication.GoalField[0],
                Pieces = new Shared.DTOs.Communication.Piece[0],
                PlayerLocation = new Location()
            };
            data.PlayerLocation.X = x;
            data.PlayerLocation.Y = y;

            return data;
        }

        private Shared.DTOs.Communication.Player GetPlayer(uint id)
        {
            Shared.DTOs.Communication.Player player = new Shared.DTOs.Communication.Player();
            player.Id = id;
            return player;
        }

        private Shared.DTOs.Communication.Piece GetPiece()
        {
            Shared.DTOs.Communication.Piece piece = new Shared.DTOs.Communication.Piece();
            piece.Type = PieceType.Unknown;
            piece.Timestamp = new System.DateTime(0);
            piece.Id = 1;
            return piece;
        }
    }

}
