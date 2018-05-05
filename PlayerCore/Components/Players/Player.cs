using PlayerCore.Base.Players;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Players
{
	public class Player : PlayerBase
	{
		#region PlayerBase
		public override Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			return Task.CompletedTask;
		}
		#endregion
		#region Player
		public Player( uint retryJoinGameInterval ) : base( retryJoinGameInterval )
		{
		}
		#endregion
	}
}
