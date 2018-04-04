using Shared.Components.Events;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;
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
		IBoardPrototypeFactory Factory { get; }
		IField GetField( uint x, uint y );
		IPiece GetPiece( ulong id );
		IPlayer GetPlayer( ulong id );
		void SetField( IField value );
		void SetPlayer( IPlayer value );
		void SetPiece( IPiece value );
		event EventHandler<FieldChangedArgs> FieldChanged;
		event EventHandler<PieceChangedArgs> PieceChanged;
		event EventHandler<PlayerChangedArgs> PlayerChanged;
	}
}
