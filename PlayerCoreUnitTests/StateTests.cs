using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlayerCore;
using Shared.Components.Events;
using Shared.Enums;
using Shared.Messages.Communication;

namespace PlayerCoreUnitTests
{
    [TestFixture]
    public partial class StateTests
    {

        private State GetState(uint id) => new State(GetGame(id), id, 0, string.Empty, new Shared.Components.Factories.BoardFactory());

        private Game GetGame(uint id)
        {

            Game game = new Game();
            game.Board = new Shared.Messages.Communication.GameBoard();
            game.Board.goalsHeight = 10;
            game.Board.tasksHeight = 10;
            game.Board.width = 10;
            game.playerId = id;
            game.PlayerLocation = new Location();
            game.Players = new Shared.Messages.Communication.Player[1];
            game.Players[0] = GetPlayer(id);
            return game;
        }

        private Data GetData()
        {
            Data data = new Data();
            data.TaskFields = new Shared.Messages.Communication.TaskField[0];
            data.GoalFields = new Shared.Messages.Communication.GoalField[0];
            data.Pieces = new Shared.Messages.Communication.Piece[0];
            data.PlayerLocation = new Location();
            return data;
        }

        private Data GetDataWithLocation(uint x, uint y)
        {
            Data data = new Data();
            data.TaskFields = new Shared.Messages.Communication.TaskField[0];
            data.GoalFields = new Shared.Messages.Communication.GoalField[0];
            data.Pieces = new Shared.Messages.Communication.Piece[0];
            data.PlayerLocation = new Location();
            data.PlayerLocation.x = x;
            data.PlayerLocation.y = y;

            return data;
        }

        private Shared.Messages.Communication.Player GetPlayer(uint id)
        {
            Shared.Messages.Communication.Player player = new Shared.Messages.Communication.Player();
            player.id = id;
            return player;
        }

        private Shared.Messages.Communication.Piece GetPiece()
        {
            Shared.Messages.Communication.Piece piece = new Shared.Messages.Communication.Piece();
            piece.type = PieceType.Unknown;
            piece.timestamp = new System.DateTime(0);
            return piece;
        }

        [Test]
        public void ReceiveData_DataEmpty_NoChange()
        {
            State state = GetState(1);
            Data data = GetData();

            state.ReceiveData(data);

            Assert.Multiple(() =>
            {
                Assert.IsNull(state.HoldingPiece);
                Assert.AreEqual(1, state.Game.Players.Length);
                Assert.AreEqual(0, state.Game.PlayerLocation.x);
                Assert.AreEqual(0, state.Game.PlayerLocation.y);
                Assert.AreEqual(10, state.Game.Board.width);
                Assert.AreEqual(10, state.Game.Board.tasksHeight);
                Assert.AreEqual(10, state.Game.Board.goalsHeight);
            });
        }


        [TestCase(0u, 0u)]
        public void ReceiveData_NewLocation_LocationChange(uint x, uint y)
        {
            State state = GetState(1);
            Data data = GetDataWithLocation(x,y);
            //data.PlayerLocation.x = x;
            //data.PlayerLocation.y = y;

            state.ReceiveData(data);
            uint X = state.Location.x;

            Assert.AreEqual(x, state.Location.x);

        }

        [TestCase(1u)]
        [TestCase(2u)]
        public void IsHoldingPiece_Test(uint id)
        {
            State state = GetState(id);
            state.Game.Players = new Shared.Messages.Communication.Player[1];
            state.Game.Players[0] = GetPlayer(id);
            Data data = GetData();

            data.Pieces = new Shared.Messages.Communication.Piece[1];
            var piece= GetPiece();
            piece.id = id;
            piece.playerId = id;
            piece.playerIdSpecified = true;
            piece.timestamp = new DateTime();
            piece.type = PieceType.Unknown;
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

            Assert.IsNull( state.HoldingPiece);
        }

        [Test]
        public void ReceiveData_EndingGame_InvokeEvent()
        {
            State state = GetState(1);
            state.Game.Players = new Shared.Messages.Communication.Player[1];
            state.Game.Players[0] = GetPlayer(1);
            Data data = GetData();
            data.gameFinished = true;
            bool wasCalled = false;
            state.EndGame += (s, a) => wasCalled = true;

            state.ReceiveData(data);

            Assert.IsTrue(wasCalled);
        }

        //[TestCase(0u, 0u)]
        //public void ReceiveData_UpdateBoard_Taskfield_PutPlayer(uint x, uint y)
        //{
        //    State state = GetState(1);
        //    Data data = GetData();

        //    Shared.Messages.Communication.TaskField taskField = new Shared.Messages.Communication.TaskField();
        //    taskField.x = x;
        //    taskField.y = y;
        //    taskField.playerIdSpecified = true;
        //    taskField.playerId = 1;
        //    taskField.distanceToPiece = 1;



        //    data.TaskFields = new Shared.Messages.Communication.TaskField[1];
        //    data.TaskFields[0] = taskField;

        //    state.ReceiveData(data);

        //    Assert.AreEqual(taskField.playerId, state.Board.GetField(x, y).Player.Id);

        //}


        [TestCase(9u, 9u)]
        [TestCase(1u,1u)]
        [TestCase(2u,5u)]
        public void ReceiveData_UpdateBoard_GoalField_PutPlayer(uint x, uint y)
        {
            State state = GetState(1);
            Data data = GetDataWithLocation(x,y);

            Shared.Messages.Communication.GoalField goalField = new Shared.Messages.Communication.GoalField();
            goalField.x = x;
            goalField.y = y;
            goalField.playerIdSpecified = true;
            goalField.playerId = 1;
            


            data.GoalFields = new Shared.Messages.Communication.GoalField[1];
            data.GoalFields[0] = goalField;

            state.ReceiveData(data);

            Assert.AreEqual(goalField.playerId, state.Board.GetField(x, y).Player.Id);

        }

    }

}
