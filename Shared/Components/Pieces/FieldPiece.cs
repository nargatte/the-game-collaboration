using Shared.Components.Fields;
using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public class FieldPiece : Piece, IFieldPiece
	{
		#region Piece
		public override IPiece ClonePiece() => CloneFieldPiece();
		#endregion
		#region IFieldPiece
		public virtual ITaskField Field { get; set; }
		public virtual IFieldPiece CloneFieldPiece()
		{
			var piece = new FieldPiece( Id, Type, Timestamp, null );
			return piece;
		}
		#endregion
		#region FieldPiece
		public FieldPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null ) : base( id, type, timestamp ) => Field = field;
		#endregion
	}
}