using Shared.Components.Players;
using System;

namespace Shared.Components.Fields
{
	public abstract class Field : IField
	{
		#region IField
		public virtual uint X { get; set; }
		public virtual uint Y { get; set; }
		public virtual DateTime Timestamp { get; set; }
		private IPlayer player;
		public virtual IPlayer Player
		{
			get => player;
			set
			{
				if( player != value )
				{
					var aPlayer = player;
					player = value;
					if( aPlayer != null && aPlayer.Field == this )
						aPlayer.Field = null;
					if( player != null )
						player.Field = this;
				}
			}
		}
		public abstract IField CloneField();
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
