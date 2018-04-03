using System;
using Shared.Enums;

namespace Shared.Components.Pieces
{
	/// <summary>
	/// immutable
	/// </summary>
	public abstract class Piece : IPiece
	{
		#region IPiece
		public virtual ulong Id { get; }
		public virtual PieceType Type { get; }
		public virtual DateTime Timestamp { get; }
		#endregion
		#region Piece
		protected Piece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default )
		{
			Id = id;
			Type = type;
			Timestamp = timestamp;
		}
		protected Piece( IPiece piece ) : this( piece.Id, piece.Type, piece.Timestamp )
		{
		}
		#endregion
	}
}
