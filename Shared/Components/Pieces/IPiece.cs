using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public interface IPiece
    {
        ulong Id { get; set; }
        PieceType Type { get; set; }
        DateTime Timestamp { get; set; }
		IPiece ClonePiece();
    }
}
