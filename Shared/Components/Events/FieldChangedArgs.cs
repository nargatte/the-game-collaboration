using System;

namespace Shared.Components.Events
{
	public class FieldChangedArgs : EventArgs
	{
		public uint X { get; }
		public uint Y { get; }
		public FieldChangedArgs( uint x, uint y )
		{
			X = x;
			Y = y;
		}
	}
}
