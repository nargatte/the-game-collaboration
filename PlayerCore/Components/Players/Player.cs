﻿using PlayerCore.Base.Players;
using Shared.DTOs.Communication;
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
			System.Console.WriteLine( $"Player sends: { Shared.Components.Serialization.Serializer.Serialize( new GetGames() ) }." );
			await Proxy.SendAsync( new GetGames(), cancellationToken ).ConfigureAwait( false );
		}
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval ) : base( retryJoinGameInterval )
		{
		}
		#endregion
	}
}
