using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System.Collections.Generic;

namespace Shared.Components.Boards
{
    public interface IBoard
    {
        uint Width { get; }
        uint TasksHeight { get; }
        uint GoalsHeight { get; }
		uint Height { get; }
		IEnumerable<IField> Fields { get; }
		IEnumerable<IPiece> Pieces { get; }
		IEnumerable<IPlayer> Players { get; }
		IField GetField( uint x, uint y );
		IPiece GetPiece( ulong id );
		IPlayer GetPlayer( ulong id );
		void SetField( uint x, uint y, IField value );
		void SetPlayer( ulong id, IPlayer value );
		void SetPiece( ulong id, IPiece value );
	}
}
