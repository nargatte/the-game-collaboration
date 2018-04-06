using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace Shared.Components.Boards
{
	public interface IBoard : IReadOnlyBoard
    {
		void SetField( IField value );
		void SetPlayer( IPlayer value );
		void SetPiece( IPiece value );
	}
}
