using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DTO = Shared.Messages.Communication;
using Config = Shared.Messages.Configuration;
using Shared.Interfaces;
using Shared.Components.Factories;
using Shared.Components.Boards;
using Shared.Components.Fields;

namespace GameMasterCoreUnitTests
{
    [TestClass]
    public class GameMasterTest
    {
        private Config.GameMasterSettings CreateEasyConfig()
        {
            return new Config.GameMasterSettings()
            {
                ActionCosts = new Config.GameMasterSettingsActionCosts()
                {
                    DiscoverDelay = 0,
                    KnowledgeExchangeDelay = 0,
                    MoveDelay = 0,
                    PickUpDelay = 0,
                    PlacingDelay = 0,
                    TestDelay = 0
                },
                GameDefinition = new Config.GameMasterSettingsGameDefinition()
                {
                    GameName = "easy",
                    BoardWidth = 2,
                    GoalAreaLength = 1,
                    Goals = new Config.GoalField[] {
                        new Config.GoalField { team = Shared.Enums.TeamColour.Blue, x = 0, y = 0, type = Shared.Enums.GoalFieldType.Goal},
                        new Config.GoalField { team = Shared.Enums.TeamColour.Red, x = 0, y = 3, type = Shared.Enums.GoalFieldType.Goal}
                    },
                    InitialNumberOfPieces = 1,
                    NumberOfPlayersPerTeam = 1,
                    ShamProbability = 0,
                    TaskAreaLength = 2
                }
            };
        }


        [TestMethod]
        public void DefaultConstructorTest()
        {
            IGameMaster gameMaster = new BlockingGameMaster();
        }

        [TestMethod]
        public void ConstructorTest_Easy()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            IReadOnlyBoard board = gameMaster.Board;

            Assert.AreEqual(board.GetPiece(1).Id,(ulong)1);
            Assert.AreEqual(board.GetPiece(1).Type, Shared.Enums.PieceType.Normal);
            Assert.IsTrue(board.GetField(0, 0) is IGoalField);
            Assert.IsTrue(board.GetField(1, 0) is IGoalField);
            Assert.IsTrue(board.GetField(0, 1) is ITaskField);
            Assert.IsTrue(board.GetField(1, 1) is ITaskField);
            Assert.IsTrue(board.GetField(0, 2) is ITaskField);
            Assert.IsTrue(board.GetField(1, 2) is ITaskField);
            Assert.IsTrue(board.GetField(0, 3) is IGoalField);
            Assert.IsTrue(board.GetField(1, 3) is IGoalField);

        }

        [TestMethod]
        public void JoinGameTest_RedLeader()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.team, joinGameMessage.preferredTeam);
            Assert.AreEqual(resultPlayer.type, joinGameMessage.preferredRole);
        }

        [TestMethod]
        public void JoinGameTest_RedMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Member,
                preferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.team, joinGameMessage.preferredTeam);
            Assert.AreEqual(resultPlayer.type, joinGameMessage.preferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueLeader()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.team, joinGameMessage.preferredTeam);
            Assert.AreEqual(resultPlayer.type, joinGameMessage.preferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Member,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.team, joinGameMessage.preferredTeam);
            Assert.AreEqual(resultPlayer.type, joinGameMessage.preferredRole);
        }

        [TestMethod]
        public void JoinGameTest_TooManyPlayers()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };

            gameMaster.PerformJoinGame(joinGameMessage);
            gameMaster.PerformJoinGame(joinGameMessage);
            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.RejectJoiningGame);
            Assert.AreEqual(((DTO.RejectJoiningGame)result).gameName, "easy");
        }
        
        [TestMethod]
        public void GetGame()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);

            Assert.AreEqual(game.playerId, playerMessage.playerId);
            Assert.AreEqual(game.Board.goalsHeight, (uint)1);
            Assert.AreEqual(game.Board.tasksHeight, (uint)2);
            Assert.AreEqual(game.Board.width, (uint)2);
            Assert.IsTrue(game.PlayerLocation.x < 2);
            Assert.IsTrue(game.PlayerLocation.y < 3);
        }

        [TestMethod]
        public void MoveTest_Up()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.AreEqual(data.PlayerLocation.x, game.PlayerLocation.x);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y + 1);
        }

        [TestMethod]
        public void MoveTest_Right()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Right,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.AreEqual(data.PlayerLocation.x, game.PlayerLocation.x + 1);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y);
        }

        [TestMethod]
        public void MoveTest_PlayerCollision()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.JoinGame joinGameMessage2 = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Red
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };
            DTO.Move moveMessage2 = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Down,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var playerMessage2 = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage2);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            var game2 = gameMaster.GetGame(playerMessage2.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            moveMessage2.playerGuid = playerMessage2.privateGuid;

            // for this specific seed player 1 starts at y=1, player 2 starts at y=3
            var data = gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage2);
            gameMaster.PerformMove(moveMessage2);
            var data2 = gameMaster.PerformMove(moveMessage2);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.AreEqual(data2.playerId, playerMessage2.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y + 1);
            Assert.AreEqual(data2.PlayerLocation.y, game2.PlayerLocation.y);
        }

        [TestMethod]
        public void MoveTest_Outside1()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Left,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.x, game.PlayerLocation.x);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y);
        }

        [TestMethod]
        public void MoveTest_Outside2()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 123455);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Down,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.x, game.PlayerLocation.x);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y);
        }

        [TestMethod]
        public void MoveTest_EnemyBase()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 123);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Move moveMessage = new DTO.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            moveMessage.playerGuid = playerMessage.privateGuid;
            gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage);
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.x, game.PlayerLocation.x);
            Assert.AreEqual(data.PlayerLocation.y, game.PlayerLocation.y + 2);
        }

        [TestMethod]
        public void DiscoverTest()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234578);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "easy",
                preferredRole = Shared.Enums.PlayerType.Leader,
                preferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.Discover discoverMessage = new DTO.Discover()
            {
                gameId = 1
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.privateGuid);
            discoverMessage.playerGuid = playerMessage.privateGuid;
            var data = gameMaster.PerformDiscover(discoverMessage);

            Assert.AreEqual(data.playerId, playerMessage.playerId);
            Assert.IsFalse(data.gameFinished);
            Assert.AreEqual(data.TaskFields.Length, 2);
            Assert.IsNull(data.GoalFields);
            Assert.IsNull(data.PlayerLocation);
        }
    }
}
