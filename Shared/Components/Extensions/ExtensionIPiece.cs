using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPiece
	{
		public static bool IsDefault( this IPiece piece ) => piece.Type == PieceType.Unknown && piece.Timestamp == default;
		public static IPiece MakePiece( this IPiece piece, ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default ) => piece.CreatePiece( id, type, timestamp );
		public static IPiece SetType( this IPiece piece, PieceType type = PieceType.Unknown ) => piece.CreatePiece( piece.Id, type, piece.Timestamp );
		public static IPiece SetTimestamp( this IPiece piece, DateTime timestamp = default ) => piece.CreatePiece( piece.Id, piece.Type, timestamp );
	}
}