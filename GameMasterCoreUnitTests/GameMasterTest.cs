using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DTO = Shared.DTO.Communication;
using Config = Shared.DTO.Configuration;
using Shared.Interfaces;
using Shared.Components.Factories;
using Shared.Components.Boards;
using Shared.Components.Fields;
using Shared.DTO.Communication;
using Shared.Enums;
using System.Collections.Generic;

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
                    Goals = new List<Config.GoalField> {
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

            Assert.AreEqual(board.GetPiece(1).Id, (ulong)1);
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
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Role, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_RedMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Member,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Role, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueLeader()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Role, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_BlueMember()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Member,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };


            DTO.PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is DTO.ConfirmJoiningGame);
            DTO.Player resultPlayer = ((DTO.ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.Team, joinGameMessage.PreferredTeam);
            Assert.AreEqual(resultPlayer.Role, joinGameMessage.PreferredRole);
        }

        [TestMethod]
        public void JoinGameTest_TooManyPlayers()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
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
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);

            Assert.AreEqual(game.PlayerId, playerMessage.PlayerId);
            Assert.AreEqual(game.Board.GoalsHeight, (uint)1);
            Assert.AreEqual(game.Board.TasksHeight, (uint)2);
            Assert.AreEqual(game.Board.Width, (uint)2);
            Assert.IsTrue(game.PlayerLocation.X < 2);
            Assert.IsTrue(game.PlayerLocation.Y < 3);
        }

        [TestMethod]
        public void MoveTest_Up()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory());
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = Shared.Enums.MoveType.Up,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.AreEqual(data.PlayerLocation.X, game.PlayerLocation.X);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y + 1);
        }

        [TestMethod]
        public void MoveTest_Right()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = MoveType.Right,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.AreEqual(data.PlayerLocation.X, game.PlayerLocation.X + 1);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y);
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
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            DTO.JoinGame joinGameMessage2 = new DTO.JoinGame()
            {
                PlayerId = 2,
                PlayerIdSpecified = true,
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Red
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = MoveType.Up,
                DirectionSpecified = true,
            };
            Move moveMessage2 = new Move()
            {
                GameId = 1,
                Direction = MoveType.Down,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var playerMessage2 = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage2);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            var game2 = gameMaster.GetGame(playerMessage2.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            moveMessage2.PlayerGuid = playerMessage2.PrivateGuid;

            // for this specific seed player 1 starts at y=1, player 2 starts at y=3
            var data = gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage2);
            gameMaster.PerformMove(moveMessage2);
            var data2 = gameMaster.PerformMove(moveMessage2);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.AreEqual(data2.PlayerId, playerMessage2.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y + 1);
            Assert.AreEqual(data2.PlayerLocation.Y, game2.PlayerLocation.Y);
        }

        [TestMethod]
        public void MoveTest_Outside1()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = MoveType.Left,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.X, game.PlayerLocation.X);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y);
        }

        [TestMethod]
        public void MoveTest_Outside2()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 123455);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = MoveType.Down,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.X, game.PlayerLocation.X);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y);
        }

        [TestMethod]
        public void MoveTest_EnemyBase()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 123);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Move moveMessage = new Move()
            {
                GameId = 1,
                Direction = MoveType.Up,
                DirectionSpecified = true,
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            moveMessage.PlayerGuid = playerMessage.PrivateGuid;
            gameMaster.PerformMove(moveMessage);
            gameMaster.PerformMove(moveMessage);
            var data = gameMaster.PerformMove(moveMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.IsNull(data.TaskFields);
            Assert.IsNull(data.GoalFields);
            Assert.AreEqual(data.PlayerLocation.X, game.PlayerLocation.X);
            Assert.AreEqual(data.PlayerLocation.Y, game.PlayerLocation.Y + 2);
        }

        [TestMethod]
        public void DiscoverTest()
        {
            IGameMaster gameMaster = new BlockingGameMaster(CreateEasyConfig(), new BoardComponentFactory(), 1234578);
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                GameName = "easy",
                PreferredRole = Shared.Enums.PlayerRole.Leader,
                PreferredTeam = Shared.Enums.TeamColour.Blue
            };
            Discover discoverMessage = new Discover()
            {
                GameId = 1
            };

            var playerMessage = (DTO.ConfirmJoiningGame)gameMaster.PerformJoinGame(joinGameMessage);
            var game = gameMaster.GetGame(playerMessage.PrivateGuid);
            discoverMessage.PlayerGuid = playerMessage.PrivateGuid;
            var data = gameMaster.PerformDiscover(discoverMessage);

            Assert.AreEqual(data.PlayerId, playerMessage.PlayerId);
            Assert.IsFalse(data.GameFinished);
            Assert.AreEqual(data.TaskFields.Count, 2);
            Assert.IsNull(data.GoalFields);
            Assert.IsNull(data.PlayerLocation);
        }
    }
}
