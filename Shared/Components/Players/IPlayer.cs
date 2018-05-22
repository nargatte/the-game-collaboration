using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace Shared.Components.Players
{
	public interface IPlayer
	{
		ulong Id { get; set; }
		TeamColour Team { get; set; }
		PlayerRole Type { get; set; }
		DateTime Timestamp { get; set; }
		IField Field { get; set; }
		IPlayerPiece Piece { get; set; }
		IPlayer ClonePlayer();
	}
}
