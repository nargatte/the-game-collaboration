using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
    public interface IPiece
    {
        ulong Id { get; }
        PieceType Type { get; }
        DateTime Timestamp { get; }
		IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp );
    }
}
