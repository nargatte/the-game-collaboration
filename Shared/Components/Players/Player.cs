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
		public virtual IField Field { get; set; }
		public virtual IPlayerPiece Piece { get; set; }
		public virtual IPlayer ClonePlayer()
		{
			var player = new Player( Id, Team, Type, Timestamp, null, null );
			var field = Field;
			var piece = Piece;
			Field = null;
			Piece = null;
			var aField = field.CloneField();
			var aPiece = piece.ClonePlayerPiece();
			Field = field;
			Piece = piece;
			player.Field = aField;
			player.Piece = aPiece;
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
