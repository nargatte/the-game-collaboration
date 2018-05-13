using System;
using PlayerCore.Interfaces;
using Shared.Components.Factories;
using Shared.Interfaces;
using Shared.Interfaces.Proxies;
using Shared.Messages.Communication;

namespace PlayerCore
{
    public class PlayerInGame
    {
        //private readonly ICommunicationServerProxy _communicationServerProxy;

        private Strategy2 Strategy { get; }

        public State State { get; }

        public PlayerInGame(IServerProxy communicationServerProxy, Game game, ulong playerId, string playerGuid, ulong gameId)
        {
            //_communicationServerProxy = communicationServerProxy;
            State = new State(game, playerId, gameId, playerGuid, new BoardFactory());
            //Strategy = new Strategy2(_communicationServerProxy, State);
        }

        public Data PerformAction()
        {
            return Strategy.PerformAction();
        }


    }
}
