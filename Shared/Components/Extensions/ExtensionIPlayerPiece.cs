using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPlayerPiece
	{
		public static bool IsDefault( this IPlayerPiece piece ) => ( piece as IPiece ).IsDefault() && piece.Player is null;
		//public static IPlayerPiece MakePlayerPiece( this IPlayerPiece piece, ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, IPlayer player = null ) => piece.CreatePlayerPiece( id, type, timestamp, player );
		//public static IPlayerPiece SetType( this IPlayerPiece piece, PieceType type = PieceType.Unknown ) => piece.CreatePlayerPiece( piece.Id, type, piece.Timestamp, piece.Player );
		//public static IPlayerPiece SetTimestamp( this IPlayerPiece piece, DateTime timestamp = default ) => piece.CreatePlayerPiece( piece.Id, piece.Type, timestamp, piece.Player );
		//public static IPlayerPiece SetPlayer( this IPlayerPiece piece, IPlayer player = null ) => piece.CreatePlayerPiece( piece.Id, piece.Type, piece.Timestamp, player );
	}
}