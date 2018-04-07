using System;
using GameMasterCore;
using Shared.Components.Factories;
using Shared.Messages.Communication;

namespace PlayerCore
{
    public class PlayerInGame
    {
        private IGameMaster GameMaster { get; }

        private Strategy Strategy { get; }

        private State State { get; }

        public PlayerInGame(IGameMaster gameMaster, Game game, ulong playerId, string playerGuid, ulong gameId, EventHandler endGame)
        {
            this.GameMaster = gameMaster;
            State = new State(game, playerId, gameId, playerGuid, new BoardFactory());
            Strategy = new Strategy(gameMaster, State);
            State.EndGame += endGame;
        }

        public void PerformAction()
        {
            Strategy.PerformAction();
        }


    }
}
