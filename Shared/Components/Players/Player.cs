using System;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;

namespace Shared.Components.Players
{
	/// <summary>
	/// immutable
	/// </summary>
	public class Player : IPlayer
	{
		#region IPlayer
		public virtual ulong Id { get; }
		public virtual TeamColour Team { get; }
		public virtual PlayerType Type { get; }
		public virtual DateTime Timestamp { get; }
		public virtual IField Field { get; }
		public virtual IPlayerPiece Piece { get; }
		#endregion
		#region Player
		public Player( ulong id, TeamColour team, PlayerType type, DateTime timestamp = default( DateTime ), IField field = null, IPlayerPiece piece = null )
		{
			Id = id;
			Team = team;
			Type = type;
			Timestamp = timestamp;
			Field = field;
			Piece = piece;
		}
		public Player( IPlayer player, DateTime timestamp = default(DateTime), IField field = null, IPlayerPiece piece = null) 
            : this( player.Id, player.Team, player.Type,
                  timestamp == default(DateTime) ? player.Timestamp : timestamp,
                  field == null ? player.Field : field,
                  piece == null ? player.Piece : piece )
		{
		}
		#endregion
	}
}
