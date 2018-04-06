using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DTO = Shared.Messages.Communication;

namespace GameMasterCoreUnitTests
{
    [TestClass]
    public class GameMasterTest
    {

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
    }
}
