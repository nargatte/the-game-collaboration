using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DTO = Shared.DTOs.Communication;
using Config = Shared.DTOs.Configuration;
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
                        new Config.GoalField { Team = Shared.Enums.TeamColour.Blue, X = 0, Y = 0, Type = Shared.Enums.GoalFieldType.Goal},
                        new Config.GoalField { Team = Shared.Enums.TeamColour.Red, X = 0, Y = 3, Type = Shared.Enums.GoalFieldType.Goal}
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Type, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_RedMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Member,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Type, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueLeader()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Type, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Member,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Type, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_TooManyPlayers()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };

            gameMaster.PerformJoinGame(joinGameMessage);
            gameMaster.PerformJoinGame(joinGameMessage);
            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.RejectJoiningGame);
            Assert.AreEqual(((DTO.RejectJoiningGame)result).GameName, "easy");
        }
        
        [TestMethod]
        public void GetGame()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);

            Assert.AreEqual(game.playerId, playerMessage.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Right,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
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
                PlayerId = 1,
                PlayerIdSpecified = true,
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.JoinGame joinGameMessage2 = new DTO.JoinGame()
            {
                PlayerId = 2,
                PlayerIdSpecified = true,
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };
            Shared.Messages.Communication.Move moveMessage2 = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Down,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var playerMessage2 = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage2);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            var game2 = gameMaster.GetGame(playerMessage2.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            moveMessage2.playerGuid = playerMessage2.PrivateGuid;

            // for this specific seed player 1 starts at y=1, player 2 starts at y=3
            var data = gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage2);
            gameMaster.PerformMove(moveMessage2);
            var data2 = gameMaster.PerformMove(moveMessage2);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
            Assert.AreEqual(data2.playerId, playerMessage2.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Left,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Down,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Move moveMessage = new Shared.Messages.Communication.Move()
            {
                gameId = 1,
                direction = Shared.Enums.MoveType.Up,
                directionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.playerGuid = playerMessage.PrivateGuid;
            gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage);
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
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
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerType.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Shared.Messages.Communication.Discover discoverMessage = new Shared.Messages.Communication.Discover()
            {
                gameId = 1
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            discoverMessage.playerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformDiscover(discoverMessage);

            Assert.AreEqual(data.playerId, playerMessage.PlayerId);
            Assert.IsFalse(data.gameFinished);
            Assert.AreEqual(data.TaskFields.Length, 2);
            Assert.IsNull(data.GoalFields);
            Assert.IsNull(data.PlayerLocation);
        }
    }
}
