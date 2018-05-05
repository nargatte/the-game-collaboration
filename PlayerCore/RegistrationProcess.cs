using System;
using PlayerCore.Interfaces;
using Shared.Enums;
using Shared.Messages.Communication;
using System.Linq;

namespace PlayerCore
{
    public class RegistrationProcess
    {
        //private ICommunicationServerProxy _communicationServerProxy;
        private readonly string _gameName;
        private readonly TeamColour _teamColour;
        private readonly PlayerType _playerType;

        public RegistrationProcess(
            //ICommunicationServerProxy communicationServerProxy,
            string gameName,
            TeamColour teamColour,
            PlayerType playerType)
        {
            //_communicationServerProxy = communicationServerProxy;
            _gameName = gameName;
            _teamColour = teamColour;
            _playerType = playerType;
        }

        public PlayerInGame Registration()
        {
            GameInfo gameInfo = null;

            do
            {
                //_communicationServerProxy.Send(new GetGames());

                //_communicationServerProxy.TryReceive(out RegisteredGames registeredGames);

                //if(registeredGames == null)
                    continue;

                //gameInfo = registeredGames.GameInfo.FirstOrDefault(g => g.gameName == _gameName);

            } while (gameInfo == null);

            /*_communicationServerProxy.Send(new JoinGame
            {
                gameName = gameInfo.gameName,
                playerIdSpecified = false,
                preferredRole = _playerType,
                preferredTeam = _teamColour
            });*/

            //_communicationServerProxy.TryReceive(out ConfirmJoiningGame confirmJoiningGame);

            //if (confirmJoiningGame == null)
                return null;

            //_communicationServerProxy.TryReceive(out Game game);

            //PlayerInGame playerInGame = new PlayerInGame(_communicationServerProxy, game, confirmJoiningGame.playerId, confirmJoiningGame.privateGuid, confirmJoiningGame.gameId);
            //return playerInGame;
        }
    }
}