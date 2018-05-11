using PlayerCore.Base.Players;
using Shared.DTOs.Communication;
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
			{
				RegisteredGames registeredGames;
				while( true )
				{
					System.Console.WriteLine( $"PLAYER sends: { Shared.Components.Serialization.Serializer.Serialize( new GetGames() ) }." );
					await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
					//registeredGames = await Proxy.TryReceiveAsync<RegisteredGames>( cancellationToken ).ConfigureAwait( false );
					//System.Console.WriteLine( $"PLAYER received: { Shared.Components.Serialization.Serializer.Serialize( registeredGames ) }." );
					await Task.Delay( TimeSpan.FromMilliseconds( RetryJoinGameInterval ), cancellationToken ).ConfigureAwait( false );
				}
			}
			//System.Console.WriteLine( $"Player sends: { Shared.Components.Serialization.Serializer.Serialize( new GetGames() ) }." );
			//await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
			/*await Task.Delay( 15000, cancellationToken );
			System.Console.WriteLine( $"Player sends: { Shared.Components.Serialization.Serializer.Serialize( new GetGames() ) }." );
			await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
			await Task.Delay( 45000, cancellationToken );*/
		}
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval ) : base( retryJoinGameInterval )
		{
		}
		#endregion
	}
}
