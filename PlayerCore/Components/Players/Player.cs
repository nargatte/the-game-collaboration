using PlayerCore.Interfaces;
using Shared.Components.Communication;
using Shared.DTOs.Configuration;
using System.Collections.Generic;

namespace PlayerCore.Components.Players
{
	public class Player : IPlayer
	{
		#region IPlayer
		public virtual void Start()
		{
		}
		public virtual uint KeepAliveInterval { get; }
		public virtual ICommunicationServerProxy Proxy { get; set; }
		#endregion
		#region Player
		public Player( uint keepAliveInterval ) => KeepAliveInterval = keepAliveInterval;
		#endregion
	}
}
