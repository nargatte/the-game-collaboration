using Shared.Components.Pieces;

namespace Shared.Components.Fields
{
    interface ITaskField : IField
    {
        int DistanceToPiece { get; }
        IPiece Piece { get; }
    }
}
