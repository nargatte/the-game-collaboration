using System;
using GameMasterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared.Messages.Communication;
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


            PlayerMessage result = gameMaster.PerformJoinGame(joinGameMessage);

            Assert.IsTrue(result is ConfirmJoiningGame);
            DTO.Player resultPlayer = ((ConfirmJoiningGame)result).PlayerDefinition;
            Assert.AreEqual(resultPlayer.team, joinGameMessage.preferredTeam);
            Assert.AreEqual(resultPlayer.type, joinGameMessage.preferredRole);
        }
    }
}
