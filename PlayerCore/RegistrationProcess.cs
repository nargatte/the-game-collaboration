using System;
using PlayerCore.Interfaces;
using Shared.Enums;
using Shared.Messages.Communication;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shared.Interfaces.Proxies;

namespace PlayerCore
{
    public class RegistrationProcess
    {
        private IServerProxy Proxy;
        private readonly string GameName;
        private readonly TeamColour TeamColour;
        private readonly PlayerRole PlayerType;
        private uint RetryJoinGameInterval;

        public event EventHandler<string> Logger = (sender, s) => { };

        public RegistrationProcess(
            IServerProxy proxy,
            string gameName,
            TeamColour teamColour,
            PlayerRole playerType,
            uint retryJoinGameInterval)
        {
            Proxy = proxy;
            GameName = gameName;
            TeamColour = teamColour;
            PlayerType = playerType;
            RetryJoinGameInterval = retryJoinGameInterval;
        }

        public async Task<PlayerInGame> Registration(CancellationToken cancellationToken)
        {
            ConfirmJoiningGame confirmJoiningGame = null;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                GameInfo gameInfo = null;
                while (true)
                {
                    await SendGetGames(cancellationToken);
                    var registeredGmes = await ReceiveRegisteredGames(cancellationToken);

                    gameInfo = registeredGmes.GameInfo?.FirstOrDefault(g => g.gameName == GameName);
                    if (gameInfo != null)
                        break;
					await Task.Delay( TimeSpan.FromMilliseconds( RetryJoinGameInterval ) );
                }

                await SendJoinGame(cancellationToken);

                confirmJoiningGame = await ReceiveConfirmJoiningGame(cancellationToken);
                if (confirmJoiningGame != null)
				{
					Proxy.UpdateLocal( Proxy.Factory.CreateIdentity( HostType.Player, confirmJoiningGame.playerId ) );
					break;
				}
            }

            Game game = await ReceiveGame(cancellationToken);

            PlayerInGame playerInGame = new PlayerInGame(Proxy, game, confirmJoiningGame.playerId, confirmJoiningGame.privateGuid, confirmJoiningGame.gameId);
            return playerInGame;
        }

        private void LogSend(object o)
        {
            //Logger(this, $"PLAYER sends: {Shared.Components.Serialization.Serializer.Serialize(o)}.");
        }

        private void LogReceive(object o)
        {
            //Logger(this, $"PLAYER received: {Shared.Components.Serialization.Serializer.Serialize(o)}.");
        }

        private async Task SendGetGames(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GetGames getGames = new GetGames();
            LogSend(getGames);
            await Proxy.SendAsync(getGames, cancellationToken).ConfigureAwait(false);
        }

        private async Task<RegisteredGames> ReceiveRegisteredGames(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            RegisteredGames registeredGames = await Proxy.TryReceiveAsync<RegisteredGames>(cancellationToken).ConfigureAwait(false);
            if(registeredGames is null)
                throw new Exception("Unexpected message received, should be RegisteredGames");
            LogReceive(registeredGames);
            return registeredGames;
        }

        private async Task SendJoinGame(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var jg = new JoinGame
            {
                gameName = GameName,
                playerIdSpecified = false,
                preferredRole = PlayerType,
                preferredTeam = TeamColour
            };
            await Proxy.SendAsync(jg, cancellationToken);
            LogSend(jg);
        }

        private async Task<ConfirmJoiningGame> ReceiveConfirmJoiningGame(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ConfirmJoiningGame confirmJoiningGame = await Proxy.TryReceiveAsync<ConfirmJoiningGame>(cancellationToken).ConfigureAwait(false);
            RejectJoiningGame receiveConfirmJoiningGame = await Proxy.TryReceiveAsync<RejectJoiningGame>(cancellationToken).ConfigureAwait(false);
            if (confirmJoiningGame is null && receiveConfirmJoiningGame is null)
                throw new Exception("Unexpected message received, should be ConfirmJoiningGame or RejectJoiningGame");
            if (confirmJoiningGame is null)
            {
                LogReceive(receiveConfirmJoiningGame);
                return null;
            }
            LogReceive(confirmJoiningGame);
            return confirmJoiningGame;
        }

        private async Task<Game> ReceiveGame(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Game game = await Proxy.TryReceiveAsync<Game>(cancellationToken);
            if (game is null)
                throw new Exception("Unexpected message received, should be Game");
            return game;
        }
    }
}