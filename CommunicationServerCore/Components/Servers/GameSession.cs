using CommunicationServerCore.Interfaces.Servers;
using Shared.DTOs.Communication;
using Shared.Interfaces.Proxies;

namespace CommunicationServerCore.Components.Servers
{
	public class GameSession : IGameSession
	{
		#region IGameSession
		public virtual string Name { get; }
		public virtual GameInfo GameInfo { get; set; }
		public virtual IClientProxy GameMaster { get; }
		#endregion
		#region GameSession
		public GameSession( string name, GameInfo gameInfo, IClientProxy gameMaster )
		{
			Name = name;
			GameInfo = gameInfo;
			GameMaster = gameMaster;
		}
		#endregion
	}
}
