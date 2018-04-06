using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public abstract class Piece : IPiece
	{
		#region IPiece
		public virtual ulong Id { get; set; }
		public virtual PieceType Type { get; set; }
		public virtual DateTime Timestamp { get; set; }
		public abstract IPiece ClonePiece();
		#endregion
		#region Piece
		protected Piece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default )
		{
			Id = id;
			Type = type;
			Timestamp = timestamp;
		}
		#endregion
	}
}
