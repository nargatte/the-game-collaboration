using CommunicationServerCore.Interfaces.Servers;
using Shared.Const;
using Shared.Interfaces.Proxies;
using System;

namespace CommunicationServerCore.Components.Servers
{
	public class PlayerSession : IPlayerSession
	{
		#region IPlayerSession
		public virtual IClientProxy Player { get; }
		public virtual ulong GameId { get; set; }
		#endregion
		#region PlayerSession
		public PlayerSession( IClientProxy player )
		{
			Player = player is null ? throw new ArgumentNullException( nameof( player ) ) : player;
			GameId = ConstHelper.AnonymousId;
		}
		#endregion
	}
}
