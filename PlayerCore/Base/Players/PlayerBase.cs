using PlayerCore.Interfaces.Players;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Base.Players
{
	public abstract class PlayerBase : IPlayer
	{
		#region IPlayer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual uint RetryJoinGameInterval { get; }
		public virtual IServerProxy Proxy { get; set; }
		#endregion
		#region PlayerBase
		protected PlayerBase( uint retryJoinGameInterval ) => RetryJoinGameInterval = retryJoinGameInterval;
		#endregion
	}
}
