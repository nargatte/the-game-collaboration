using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Pieces
{
	public class PlayerPiece : Piece, IPlayerPiece
	{
		#region Piece
		public override IPiece ClonePiece() => ClonePlayerPiece();
		#endregion
		#region IPlayerPiece
		public virtual IPlayer Player { get; set; }
		public virtual IPlayerPiece ClonePlayerPiece()
		{
			var piece = new PlayerPiece( Id, Type, Timestamp, null );
			return piece;
		}
		#endregion
		#region PlayerPiece
		public PlayerPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, IPlayer player = null ) : base( id, type, timestamp ) => Player = player;
		#endregion
	}
}
