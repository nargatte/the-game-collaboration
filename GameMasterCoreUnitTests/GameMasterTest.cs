using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DTO = Shared.Messages.Communication;
using Config = Shared.Messages.Configuration;
using Shared.Interfaces;

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
                    BoardWidth = 1,
                    GoalAreaLength = 1,
                    Goals = new Config.GoalField[] {
                        new Config.GoalField { team = Shared.Enums.TeamColour.Blue, x = 0, y = 0, type = Shared.Enums.GoalFieldType.Goal},
                        new Config.GoalField { team = Shared.Enums.TeamColour.Red, x = 0, y = 2, type = Shared.Enums.GoalFieldType.Goal}
                    },
                    InitialNumberOfPieces = 1,
                    NumberOfPlayersPerTeam = 1,
                    ShamProbability = 0,
                    TaskAreaLength = 1
                }
            };
        }


        [TestMethod]
        public void DefaultConstructorTest()
        {
            IGameMaster gameMaster = new BlockingGameMaster();
        }

        [TestMethod]
        public void JoinGameTest_RedLeader()
        {
            IGameMaster gameMaster = new BlockingGameMaster();
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "default game",
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
            IGameMaster gameMaster = new BlockingGameMaster();
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "default game",
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
            IGameMaster gameMaster = new BlockingGameMaster();
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "default game",
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
            IGameMaster gameMaster = new BlockingGameMaster();
            DTO.JoinGame joinGameMessage = new DTO.JoinGame()
            {
                gameName = "default game",
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
