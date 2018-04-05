using GameMasterCore;
using Shared.Messages.Communication;

namespace PlayerCore
{
    class PlayerInGame
    {
        private IGameMaster GameMaster { get; }

        private Strategy Strategy { get; }

        private State State { get; }

        public PlayerInGame(IGameMaster gameMaster, Game game, uint playerId, string playerGuid, uint gameId)
        {
            this.GameMaster = gameMaster;
            State = new State(game, playerId, gameId, playerGuid);
            Strategy = new Strategy(gameMaster, State);
        }

        public void PerformAction()
        {
            Strategy.PerformAction();
        }


    }
}
