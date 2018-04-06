using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIFieldPiece
	{
		public static bool IsDefault( this IFieldPiece piece ) => ( piece as IPiece ).IsDefault() && piece.Field is null;
		//public static IFieldPiece MakeFieldPiece( this IFieldPiece piece, ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null ) => piece.CreateFieldPiece( id, type, timestamp, field );
		//public static IFieldPiece SetType( this IFieldPiece piece, PieceType type = PieceType.Unknown ) => piece.CreateFieldPiece( piece.Id, type, piece.Timestamp, piece.Field );
		//public static IFieldPiece SetTimestamp( this IFieldPiece piece, DateTime timestamp = default ) => piece.CreateFieldPiece( piece.Id, piece.Type, timestamp, piece.Field );
		//public static IFieldPiece SetField( this IFieldPiece piece, ITaskField field = null ) => piece.CreateFieldPiece( piece.Id, piece.Type, piece.Timestamp, field );
	}
}