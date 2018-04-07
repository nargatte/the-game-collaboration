using Shared.Enums;
using System;

namespace Shared.Components.Events
{
	public class LogArgs : EventArgs
	{
		string Type { get; }
		DateTime Timestamp { get; }
		ulong GameId { get; }
		ulong PlayerId { get; }
		string PlayerGuid { get; }
		TeamColour Colour { get; }
		PlayerType Role { get; }
		public LogArgs( string type, DateTime timestamp, ulong gameId, ulong playerId, string playerGuid, TeamColour colour, PlayerType role )
		{
			Type = type;
			Timestamp = timestamp;
			GameId = gameId;
			PlayerId = playerId;
			PlayerGuid = playerGuid;
			Colour = colour;
			Role = role;
		}
	}
}
