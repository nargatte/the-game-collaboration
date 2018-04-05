using System;

namespace Shared.Components.Events
{
	public class PieceChangedArgs : EventArgs
	{
		public ulong Id { get; }
		public PieceChangedArgs( ulong id ) => Id = id;
	}
}
