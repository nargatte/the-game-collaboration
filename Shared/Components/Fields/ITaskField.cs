using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Fields
{
    public interface ITaskField : IField
    {
        int DistanceToPiece { get; }
        IFieldPiece Piece { get; }
		ITaskField CreateTaskField( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece );
	}
}
