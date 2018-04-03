using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	/// <summary>
	/// immutable
	/// </summary>
	public class PlayerPiece : Piece, IPlayerPiece
	{
		#region Piece
		public override IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp ) => new PlayerPiece( id, type, timestamp );
		#endregion
		#region IPlayerPiece
		public virtual IPlayer Player { get; }
		public virtual IPlayerPiece CreatePlayerPiece( ulong id, PieceType type, DateTime timestamp, IPlayer player ) => new PlayerPiece( id, type, timestamp, player );
		#endregion
		#region PlayerPiece
		public PlayerPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, IPlayer player = null ) : base( id, type, timestamp ) => Player = player;
		#endregion
	}
}
