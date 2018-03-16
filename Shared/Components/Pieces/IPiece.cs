using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
    interface IPiece
    {
        ulong Id { get; }
        PieceType Type { get; }
        DateTime Timestamp { get; }
        ulong? PlayerId { get; }
    }
}
