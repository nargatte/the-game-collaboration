using System;
using Shared.Components.Factories;
using Shared.Interfaces;
using Shared.Messages.Communication;

namespace PlayerCore
{
    public class PlayerInGame
    {
        private IGameMaster GameMaster { get; }

        private Strategy2 Strategy { get; }

        public State State { get; }

        public PlayerInGame(IGameMaster gameMaster, Game game, ulong playerId, string playerGuid, ulong gameId, EventHandler endGame)
        {
            this.GameMaster = gameMaster;
            State = new State(game, playerId, gameId, playerGuid, new BoardFactory());
            Strategy = new Strategy2(gameMaster, State);
            State.EndGame += endGame;
        }

        public Data PerformAction()
        {
            return Strategy.PerformAction();
        }


    }
}
