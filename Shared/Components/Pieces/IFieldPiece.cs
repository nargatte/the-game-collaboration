using Shared.Components.Fields;
using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public interface IFieldPiece : IPiece
	{
		ITaskField Field { get; }
		IFieldPiece CreateFieldPiece( ulong id, PieceType type, DateTime timestamp, ITaskField field );
	}
}
