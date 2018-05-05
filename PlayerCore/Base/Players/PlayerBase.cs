using PlayerCore.Interfaces.Players;

namespace PlayerCore.Base.Players
{
	public abstract class PlayerBase : IPlayer
	{
		#region IPlayer
		public virtual uint RetryJoinGameInterval { get; }
		#endregion
		#region PlayerBase
		protected PlayerBase( uint retryJoinGameInterval ) => RetryJoinGameInterval = retryJoinGameInterval;
		#endregion
	}
}
