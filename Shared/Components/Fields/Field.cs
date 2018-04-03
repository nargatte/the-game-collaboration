using Shared.Components.Players;
using System;

namespace Shared.Components.Fields
{
	/// <summary>
	/// immutable
	/// </summary>
	public abstract class Field : IField
	{
		#region IField
		public virtual uint X { get; }
		public virtual uint Y { get; }
		public virtual DateTime Timestamp { get; }
		public virtual IPlayer Player { get; }
		public abstract IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player );
		#endregion
		#region Field
		protected Field( uint x, uint y, DateTime timestamp = default, IPlayer player = null )
		{
			X = x;
			Y = y;
			Timestamp = timestamp;
			Player = player;
		}
		#endregion
	}
}
