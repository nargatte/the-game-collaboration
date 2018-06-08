using System;
using System.Collections.Generic;
using NUnit.Framework;
using PlayerCore;
using Shared.Components.Events;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.DTO.Communication;
using Shared.Enums;

namespace PlayerCoreUnitTests
{
    [TestFixture]
    public partial class StateTests
    {
        [Test]
        public void ReceiveData_DataEmpty_NoChange()
        {
            var state = GetState(1);
            var data = GetData();

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
            var state = GetStateWithLocation(1, x, y);
            var data = GetDataWithLocation(x, y);

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
            var state = GetState(id);
			state.Game.Players = new List<Shared.DTO.Communication.Player>
			{
				GetPlayer(id)
			};
            var data = GetData();
			var piece = GetPiece();
			piece.Id = id;
			piece.PlayerId = id;
			piece.Timestamp = new DateTime();
			piece.Type = PieceType.Unknown;
			data.Pieces = new List<Shared.DTO.Communication.Piece>
			{
				piece
			};



            state.ReceiveData(data);

            Assert.AreSame(piece, state.HoldingPiece);

        }

        [Test]
        public void IsNotHoldingPiece_Test()
        {

            var state = GetState(1);
            var data = GetData();
            state.ReceiveData(data);

            Assert.IsNull(state.HoldingPiece);
        }

        [Test]
        public void ReceiveData_EndingGame_InvokeEvent()
        {
            var state = GetState(1);
            state.Game.Players = new List<Shared.DTO.Communication.Player>
			{
				GetPlayer(1)
			};
            var data = GetData();
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
            var state = GetState(1);
            var data = GetData();

			var taskField = new Shared.DTO.Communication.TaskField
			{
				X = x,
				Y = y,
				PlayerId = 1,
				DistanceToPiece = 1
			};
			data.TaskFields = new List<Shared.DTO.Communication.TaskField>
				{
				taskField
			};

            state.ReceiveData(data);

            Assert.AreEqual(taskField.PlayerId, state.Board.GetPlayer(1).Id);

        }


        [TestCase(1u, 25u)]
        [TestCase(5u,27u)]
        [TestCase(9u,28u)]
        public void ReceiveData_UpdateBoard_GoalField_PutPlayer(uint x, uint y)
        {
            var state = GetState(1);
            var data = GetDataWithLocation(x, y);

            var goalField = new Shared.DTO.Communication.GoalField
            {
                X = x,
                Y = y,
                PlayerId = 1
            };



			data.GoalFields = new List<Shared.DTO.Communication.GoalField>
			{
				goalField
			};
            state.ReceiveData(data);

            Assert.AreEqual(goalField.PlayerId, state.Board.GetPlayer(1).Id);

        }

        [TestCase(5u, 15u)]
        [TestCase(3u, 11u)]
        [TestCase(0u, 17u)]
        public void Receive_Data_Put_Piece(uint x,uint y)
        {
            var state = GetState(1);
            var data = GetDataWithPiece(x,y);

            var piece = GetPiece();

            state.ReceiveData(data);

            Assert.AreEqual(piece.Id, state.Board.GetPiece(piece.Id).Id);

        }

        private State GetState(uint id) => new State(GetGame(id), id, 0, string.Empty, new Shared.Components.Factories.BoardFactory());

        private State GetStateWithLocation(uint id, uint x, uint y) => new State(GetGameWithLocation(id,x, y), id, 0, string.Empty, new Shared.Components.Factories.BoardFactory());

        private Game GetGame(uint id)
        {
            var game = new Game
            {
                Board = new Shared.DTO.Communication.GameBoard
                {
                    GoalsHeight = 10,
                    TasksHeight = 10,
                    Width = 10
                },
                PlayerId = id,
                PlayerLocation = new Location(),
                
                Players = new List<Shared.DTO.Communication.Player>
				{
					GetPlayer( id )
				}
            };
            
            return game;
        }

        private Game GetGameWithLocation(uint id, uint x, uint y)
        {
			var game = new Game
			{
				Board = new Shared.DTO.Communication.GameBoard
				{
					GoalsHeight = 10,
					TasksHeight = 10,
					Width = 10
				},
				PlayerId = id,
				PlayerLocation = new Location
				{
					X = x,
					Y = y
				},
				Players = new List<Shared.DTO.Communication.Player>
			{
				GetPlayer(id)
			}
			};
			return game;
        }

       

        private Data GetData()
        {
			var data = new Data
			{
				TaskFields = new List<Shared.DTO.Communication.TaskField>(),
				GoalFields = new List<Shared.DTO.Communication.GoalField>(),
				Pieces = new List<Shared.DTO.Communication.Piece>(),
				PlayerLocation = new Location()
			};
			return data;
        }

		private Data GetDataWithPiece( uint x, uint y )
		{
			var data = new Data
			{
				TaskFields = new List<Shared.DTO.Communication.TaskField>
			{
				new Shared.DTO.Communication.TaskField
				{
					PieceId = 1,
					X = x,
					Y = y
				}
		},


				GoalFields = new List<Shared.DTO.Communication.GoalField>(),
				Pieces = new List<Shared.DTO.Communication.Piece>
				{
				GetPiece()
			},
				PlayerLocation = new Location
				{
					X = x,
					Y = y
				}
			};

			return data;
        }

        private Data GetDataWithLocation(uint x, uint y)
        {
            var data = new Data
            {
                TaskFields = new List<Shared.DTO.Communication.TaskField>(),
                GoalFields = new List<Shared.DTO.Communication.GoalField> (),
                Pieces = new List<Shared.DTO.Communication.Piece> (),
                PlayerLocation = new Location()
            };
            data.PlayerLocation.X = x;
            data.PlayerLocation.Y = y;

            return data;
        }

        private Shared.DTO.Communication.Player GetPlayer(uint id)
        {
			var player = new Shared.DTO.Communication.Player
			{
				Id = id
			};
			return player;
        }

        private Shared.DTO.Communication.Piece GetPiece()
        {
			var piece = new Shared.DTO.Communication.Piece
			{
				Type = PieceType.Unknown,
				Timestamp = new System.DateTime( 0 ),
				Id = 1
			};
			return piece;
        }
    }

}
