using System;
using PlayerCore.Interfaces;
using Shared.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shared.Interfaces.Proxies;
using Shared.DTOs.Communication;

namespace PlayerCore
{
    public class RegistrationProcess
    {
        private IServerProxy Proxy;
        private readonly string GameName;
        private readonly TeamColour TeamColour;
        private readonly Shared.Enums.PlayerRole PlayerType;
        private uint RetryJoinGameInterval;

        public event EventHandler<string> Logger = (sender, s) => { };

        public RegistrationProcess(
            IServerProxy proxy,
            string gameName,
            TeamColour teamColour,
            Shared.Enums.PlayerRole playerType,
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

                    gameInfo = registeredGmes.GameInfo?.FirstOrDefault(g => g.GameName == GameName);
                    if (gameInfo != null)
                        break;
					await Task.Delay( TimeSpan.FromMilliseconds( RetryJoinGameInterval ) );
                }

                await SendJoinGame(cancellationToken);

                confirmJoiningGame = await ReceiveConfirmJoiningGame(cancellationToken);
                if (confirmJoiningGame != null)
				{
					Proxy.UpdateLocal( Proxy.Factory.CreateIdentity( HostType.Player, confirmJoiningGame.PlayerId ) );
					break;
				}
            }

            Game game = await ReceiveGame(cancellationToken);

            PlayerInGame playerInGame = new PlayerInGame(Proxy, game, confirmJoiningGame.PlayerId, confirmJoiningGame.PrivateGuid, confirmJoiningGame.GameId);
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
            while (registeredGames == null)
            {
                Proxy.Discard();
                registeredGames = await Proxy.TryReceiveAsync<RegisteredGames>(cancellationToken).ConfigureAwait(false);
            }
            LogReceive(registeredGames);
            return registeredGames;
        }

        private async Task SendJoinGame(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var jg = new JoinGame
            {
                GameName = GameName,
                PlayerIdSpecified = false,
                PreferredRole = PlayerType,
                PreferredTeam = TeamColour
            };
            await Proxy.SendAsync(jg, cancellationToken);
            LogSend(jg);
        }

        private async Task<ConfirmJoiningGame> ReceiveConfirmJoiningGame(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ConfirmJoiningGame confirmJoiningGame = await Proxy.TryReceiveAsync<ConfirmJoiningGame>(cancellationToken).ConfigureAwait(false);
            RejectJoiningGame receiveConfirmJoiningGame = await Proxy.TryReceiveAsync<RejectJoiningGame>(cancellationToken).ConfigureAwait(false);
            while (confirmJoiningGame == null && receiveConfirmJoiningGame == null)
            {
                Proxy.Discard();
                confirmJoiningGame = await Proxy.TryReceiveAsync<ConfirmJoiningGame>(cancellationToken).ConfigureAwait(false);
                receiveConfirmJoiningGame = await Proxy.TryReceiveAsync<RejectJoiningGame>(cancellationToken).ConfigureAwait(false);
            }
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
            while (game == null)
            {
                Proxy.Discard();
                game = await Proxy.TryReceiveAsync<Game>(cancellationToken);
            }
            if (game is null)
                throw new Exception("Unexpected message received, should be Game");
            return game;
        }
    }
}