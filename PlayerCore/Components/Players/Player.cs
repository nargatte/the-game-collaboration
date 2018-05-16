using PlayerCore.Base.Players;
using Shared.DTOs.Communication;
using Shared.Enums;
using Shared.Messages.Communication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Players
{
	public class Player : PlayerBase
	{
		#region PlayerBase
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			//RegisteredGames registeredGames;
			//while( true )
			//{
			//	await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
			//	registeredGames = await Proxy.TryReceiveAsync<RegisteredGames>( cancellationToken ).ConfigureAwait( false );
			//	await Task.Delay( TimeSpan.FromMilliseconds( RetryJoinGameInterval ), cancellationToken ).ConfigureAwait( false );
			//}
            RegistrationProcess registrationProcess = new RegistrationProcess(Proxy, GameName, Team, Role, RetryJoinGameInterval);
		    PlayerInGame playerInGame = await registrationProcess.Registration(cancellationToken).ConfigureAwait(false);
            bool endGame = false;
            playerInGame.State.EndGame += (e, s) => endGame = true;
            do
            {
                cancellationToken.ThrowIfCancellationRequested();

                await playerInGame.PerformAction(cancellationToken);
                Data data = await Proxy.TryReceiveAsync<Data>(cancellationToken);
                if (data is null) throw new NotImplementedException("Only Data is served now");
                playerInGame.State.ReceiveData(data);

            } while (!endGame);
        }
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval, string gameName, TeamColour team, PlayerType role ) : base( retryJoinGameInterval, gameName, team, role )
		{
		}
		#endregion
	}
}
