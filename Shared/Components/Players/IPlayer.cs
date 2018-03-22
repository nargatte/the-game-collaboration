using Shared.Components.Fields;
using Shared.Components.Pieces;
using System;

namespace Shared.Components.Players
{
	public interface IPlayer
	{
		ulong Id { get; }
		DateTime Timestamp { get; }
		IField Field { get; }
		IPlayerPiece Piece { get; }
	}
}
