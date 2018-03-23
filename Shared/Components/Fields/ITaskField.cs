using Shared.Components.Pieces;

namespace Shared.Components.Fields
{
    public interface ITaskField : IField
    {
        int DistanceToPiece { get; }
        IPiece Piece { get; }
    }
}
