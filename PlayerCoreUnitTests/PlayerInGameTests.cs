using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PlayerCore;
using Shared.Components.Boards;
using Shared.Components.Events;
using Shared.Enums;
using Shared.Interfaces;
using Shared.Messages.Communication;

namespace PlayerCoreUnitTests
{
    [TestFixture]
    class PlayerInGameTests
    {
        class MockGameMaster : IGameMaster
        {
            public Data PerformMove(Move moveRequest)
            {
                return new Data
                {
                    playerId = 1,
                    PlayerLocation = new Location
                    {
                        x = 0,
                        y = 5,
                    },
                    TaskFields = new TaskField[]
                    {
                        new TaskField
                        {
                            x = 0,
                            y = 5,
                            distanceToPiece = 4,
                            timestamp = DateTime.Now,
                            playerIdSpecified = true,
                            playerId = 0
                        }
                    }
                };
            }

            public Data PerformDiscover(Discover discoverRequest)
            {
                throw new NotImplementedException();
            }

            public Data PerformPickUp(PickUpPiece pickUpRequest)
            {
                throw new NotImplementedException();
            }

            public Data PerformTestPiece(TestPiece testPieceRequest)
            {
                throw new NotImplementedException();
            }

            public Data PerformPlace(PlacePiece placeRequest)
            {
                throw new NotImplementedException();
            }

            public Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest)
            {
                throw new NotImplementedException();
            }

            public RegisteredGames PerformConfirmGameRegistration()
            {
                throw new NotImplementedException();
            }

            public PlayerMessage PerformJoinGame(JoinGame joinGame)
            {
                throw new NotImplementedException();
            }

            public event EventHandler<LogArgs> Log;
            public IReadOnlyBoard Board { get; }
            public Game GetGame(string guid)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void MoveAndPervorm()
        {
            var p = new PlayerInGame(new MockGameMaster(), new Game
            {
                Board = new GameBoard
                {
                    goalsHeight = 3,
                    tasksHeight = 5,
                    width = 5
                },
                playerId = 0,
                PlayerLocation = new Location
                {
                    x = 4,
                    y = 0
                },
                Players = new Player[]
                {
                    new Player
                    {
                        id = 0,
                        team = TeamColour.Blue,
                        type = PlayerType.Leader
                    }
                }
            },
            0,
            "",
            0,
            null);
            var d = p.PerformAction();
            p.State.ReceiveData(d);
            var d2 = p.PerformAction();
        }
    }
}
