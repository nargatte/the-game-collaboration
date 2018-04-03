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
		#region Piece
		public override IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp ) => new FieldPiece( id, type, timestamp );
		#endregion
		#region IFieldPiece
		public virtual ITaskField Field { get; }
		public virtual IFieldPiece CreateFieldPiece( ulong id, PieceType type, DateTime timestamp, ITaskField field ) => new FieldPiece( id, type, timestamp, field );
		#endregion
		#region FieldPiece
		public FieldPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null ) : base( id, type, timestamp ) => Field = field;
		#endregion
	}
}