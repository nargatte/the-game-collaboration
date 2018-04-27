using System.Collections.Generic;
using CommunicationServerCore.Interfaces;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : ICommunicationServer
	{
		#region ICommunicationServer
		public virtual IEnumerable<IGameMasterProxy> GameMasterProxies => throw new System.NotImplementedException();
		public virtual IEnumerable<IPlayerProxy> PlayerProxies => throw new System.NotImplementedException();
		#endregion
	}
}
