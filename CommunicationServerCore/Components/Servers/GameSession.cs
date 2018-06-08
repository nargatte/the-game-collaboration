using CommunicationServerCore.Interfaces.Servers;
using Shared.DTO.Communication;
using Shared.Interfaces.Proxies;
using System;
using System.Collections.Concurrent;

namespace CommunicationServerCore.Components.Servers
{
	public class GameSession : IGameSession
	{
		#region IGameSession
		public virtual string Name { get; }
		public virtual GameInfo GameInfo { get; set; }
		public virtual IClientProxy GameMaster { get; }
		public virtual ConcurrentDictionary<ulong, ulong> Players { get; } = new ConcurrentDictionary<ulong, ulong>();
		#endregion
		#region GameSession
		public GameSession( string name, GameInfo gameInfo, IClientProxy gameMaster )
		{
			Name = name is null ? throw new ArgumentNullException( nameof( name ) ) : name;
			GameInfo = gameInfo is null ? throw new ArgumentNullException( nameof( gameInfo ) ) : gameInfo;
			GameMaster = gameMaster is null ? throw new ArgumentNullException( nameof( gameMaster ) ) : gameMaster;
		}
		#endregion
	}
}
