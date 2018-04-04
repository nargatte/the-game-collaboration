using System;

namespace Shared.Components.Events
{
	public class PlayerChangedArgs : EventArgs
	{
		public ulong Id { get; }
		public PlayerChangedArgs( ulong id ) => Id = id;
	}
}
