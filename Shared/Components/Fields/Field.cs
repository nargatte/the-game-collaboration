using System;
using Shared.Components.Players;

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
		#endregion
		#region Field
		protected Field( uint x, uint y, DateTime timestamp = default( DateTime ), IPlayer player = null )
		{
			X = x;
			Y = y;
			Timestamp = timestamp;
			Player = player;
		}
		protected Field( IField field ) : this( field.X, field.Y, field.Timestamp, field.Player )
		{
		}
		#endregion
	}
}
