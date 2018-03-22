using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System.Collections.Generic;

namespace Shared.Components.Boards
{
    public interface IBoard
    {
        uint Width { get; }
        uint Height { get; }
        uint TasksHeight { get; }
        uint GoalsHeight { get; }
		IField GetField( uint x, uint y );
		void SetField( uint x, uint y, IField value );
		IEnumerable<IField> Fields { get; }
		IPlayer GetPlayer( ulong id );
		void SetPlayer( ulong id, IPlayer value );
		IEnumerable<IPlayer> Players { get; }
		IPiece GetPiece( ulong id );
		void SetPiece( ulong id, IPiece value );
		IEnumerable<IPiece> Pieces { get; }
	}
}
