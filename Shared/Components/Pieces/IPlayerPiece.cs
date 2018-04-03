using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public interface IPlayerPiece : IPiece
	{
		IPlayer Player { get; }
		IPlayerPiece CreatePlayerPiece( ulong id, PieceType type, DateTime timestamp, IPlayer player );
	}
}
