using PlayerCore.Interfaces.Players;
using Shared.Enums;
using Shared.Interfaces.Proxies;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Base.Players
{
	public abstract class PlayerBase : IPlayer
	{
		#region IPlayer
		public abstract Task RunAsync( CancellationToken cancellationToken );
		public virtual uint RetryJoinGameInterval { get; }
		public virtual string GameName { get; }
		public virtual TeamColour Team { get; }
		public virtual PlayerType Role { get; }
		public virtual IServerProxy Proxy { get; set; }
		#endregion
		#region PlayerBase
		protected PlayerBase( uint retryJoinGameInterval, string gameName, TeamColour team, PlayerType role )
		{
			RetryJoinGameInterval = retryJoinGameInterval;
			GameName = gameName is null ? throw new ArgumentNullException( nameof( gameName ) ) : gameName;
			Team = team;
			Role = role;
		}
		#endregion
	}
}
