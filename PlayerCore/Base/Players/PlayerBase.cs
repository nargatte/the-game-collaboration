using PlayerCore.Interfaces.Players;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Base.Players
{
	public abstract class PlayerBase : IPlayer
	{
		#region IPlayer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual uint RetryJoinGameInterval { get; }
		#endregion
		#region PlayerBase
		protected PlayerBase( uint retryJoinGameInterval ) => RetryJoinGameInterval = retryJoinGameInterval;
		#endregion
	}
}
