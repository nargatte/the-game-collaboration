using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

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
		public virtual IPlayer CreatePlayer( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece ) => new Player( id, team, type, timestamp, field, piece );
		#endregion
		#region Player
		public Player( ulong id, TeamColour team, PlayerType type, DateTime timestamp = default, IField field = null, IPlayerPiece piece = null )
		{
			Id = id;
			Team = team;
			Type = type;
			Timestamp = timestamp;
			Field = field;
			Piece = piece;
		}
		#endregion
	}
}
