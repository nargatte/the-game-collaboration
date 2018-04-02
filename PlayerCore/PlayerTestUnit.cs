using GameMasterCore;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCore
{
    class PlayerTestUnit
    {
        private IGameMaster gameMaster { get; }

        private Strategy strategy { get; }

        private State state { get; }

        public PlayerTestUnit(IGameMaster gameMaster, Game game, uint playerId, string playerGuid, uint gameId)
        {
            this.gameMaster = gameMaster;
            state = new State(game, playerId, gameId, playerGuid);
            strategy = new Strategy(gameMaster, state);
        }

        public void PerformAction()
        {
            strategy.PerfirmAction();
        }


    }
}
