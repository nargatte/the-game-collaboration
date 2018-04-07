using Shared.Enums;
using System;

namespace Shared.Components.Events
{
	public class LogArgs : EventArgs
	{
		public string Type { get; }
		public DateTime Timestamp { get; }
		public ulong GameId { get; }
		public ulong PlayerId { get; }
		public string PlayerGuid { get; }
		public TeamColour Colour { get; }
		public PlayerType Role { get; }
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
