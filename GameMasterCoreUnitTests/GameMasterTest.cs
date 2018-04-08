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
    }
}
