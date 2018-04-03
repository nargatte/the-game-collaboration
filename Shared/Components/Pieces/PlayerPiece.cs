using System;
using Shared.Components.Players;
using Shared.Enums;

namespace Shared.Components.Pieces
{
	/// <summary>
	/// immutable
	/// </summary>
	public class PlayerPiece : Piece, IPlayerPiece
	{
		#region IPlayerPiece
		public virtual IPlayer Player { get; }
		#endregion
		#region PlayerPiece
		public PlayerPiece( ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, IPlayer player = null ) : base( id, type, timestamp ) => Player = player;

		public override IPiece CreatePiece( ulong id, PieceType type, DateTime timestamp ) => throw new NotImplementedException();
		#endregion
	}
}
