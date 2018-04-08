using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace Shared.Components.Players
{
	public class Player : IPlayer
	{
		#region IPlayer
		public virtual ulong Id { get; set; }
		public virtual TeamColour Team { get; set; }
		public virtual PlayerType Type { get; set; }
		public virtual DateTime Timestamp { get; set; }
		private IField field;
		public virtual IField Field
		{
			get => field;
			set
			{
				if( field != value )
				{
					var aField = field;
					field = value;
					if( aField != null && aField.Player == this )
						aField.Player = null;
					if( field != null )
						field.Player = this;
				}
			}
		}
		private IPlayerPiece piece;
		public virtual IPlayerPiece Piece
		{
			get => piece;
			set
			{
				if( piece != value )
				{
					var aPiece = piece;
					piece = value;
					if( aPiece != null && aPiece.Player == this )
						aPiece.Player = null;
					if( piece != null )
						piece.Player = this;
				}
			}
		}
		public virtual IPlayer ClonePlayer()
		{
			var player = new Player( Id, Team, Type, Timestamp, null, null );
			this.Clone( player );
			return player;
		}
		#endregion
		#region Player
		public Player( ulong id, TeamColour team, PlayerType type, DateTime timestamp = default, IField field = null, IPlayerPiece piece = null )
		{
			Id = id;
			Team = team;
			Type = type;
			Timestamp = timestamp;
			Field = field;
			Piece = piece;
		}
		#endregion
	}
}
