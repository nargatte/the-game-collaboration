using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace Shared.Components.Players
{
	public interface IPlayer
	{
		ulong Id { get; }
		TeamColour Team { get; }
		PlayerType Type { get; }
		DateTime Timestamp { get; }
		IField Field { get; }
		IPlayerPiece Piece { get; }
		IPlayer CreatePlayer( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece );
	}
}
