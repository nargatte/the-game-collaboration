using PlayerCore.Base.Players;
using Shared.DTOs.Communication;
using Shared.Enums;
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
			RegisteredGames registeredGames;
			while( true )
			{
				await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
				registeredGames = await Proxy.TryReceiveAsync<RegisteredGames>( cancellationToken ).ConfigureAwait( false );
				await Task.Delay( TimeSpan.FromMilliseconds( RetryJoinGameInterval ), cancellationToken ).ConfigureAwait( false );
			}
		}
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval, string gameName, TeamColour team, PlayerType role ) : base( retryJoinGameInterval, gameName, team, role )
		{
		}
		#endregion
	}
}
