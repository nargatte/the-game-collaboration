using Shared.Components.Extensions;
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
        private ITaskField field;
        public virtual ITaskField Field
        {
            get => field;
            set
            {
                if (field != value)
                {
                    var aField = field;
                    field = value;
                    if (aField != null)
                        aField.Piece = null;
                    if (field != null)
                        field.Piece = this;
                }
            }
        }
        public virtual IFieldPiece CloneFieldPiece()
        {
            var piece = new FieldPiece(Id, Type, Timestamp, null);
            this.Clone(piece);
            return piece;
        }
        #endregion
        #region FieldPiece
        public FieldPiece(ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null) : base(id, type, timestamp) => Field = field;
        #endregion
    }
}