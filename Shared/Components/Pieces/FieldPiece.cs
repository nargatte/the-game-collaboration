using System;
using Shared.Components.Fields;
using Shared.Enums;

namespace Shared.Components.Pieces
{
	/// <summary>
	/// immutable
	/// </summary>
	public class FieldPiece : Piece, IFieldPiece
	{
		#region IFieldPiece
		public virtual ITaskField Field { get; }
		#endregion
		#region FieldPiece
		public FieldPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null ) : base( id, type, timestamp ) => Field = field;
		public FieldPiece( IFieldPiece piece ) : base( piece ) => Field = piece.Field;
		#endregion
	}
}