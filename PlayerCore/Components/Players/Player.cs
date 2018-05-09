using PlayerCore.Base.Players;
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
			await Proxy.SendAsync( new GetGames(), cancellationToken );
		}
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval ) : base( retryJoinGameInterval )
		{
		}
		#endregion
	}
}
