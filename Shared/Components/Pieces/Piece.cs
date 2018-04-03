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
		public abstract IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp );
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
