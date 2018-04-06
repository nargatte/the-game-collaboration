using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionITaskField
	{
		public static bool IsDefault( this ITaskField field ) => ( field as IField ).IsDefault() && field.DistanceToPiece == -1 && field.Piece is null;
		//public static ITaskField MakeTaskField( this ITaskField field, uint x, uint y, DateTime timestamp = default, IPlayer player = null, int distanceToPiece = -1, IFieldPiece piece = null ) => field.CreateTaskField( x, y, timestamp, player, distanceToPiece, piece );
		//public static ITaskField SetTimestamp( this ITaskField field, DateTime timestamp = default ) => field.CreateTaskField( field.X, field.Y, timestamp, field.Player, field.DistanceToPiece, field.Piece );
		//public static ITaskField SetPlayer( this ITaskField field, IPlayer player = null ) => field.CreateTaskField( field.X, field.Y, field.Timestamp, player, field.DistanceToPiece, field.Piece );
		//public static ITaskField SetDistanceToPiece( this ITaskField field, int distanceToPiece = -1 ) => field.CreateTaskField( field.X, field.Y, field.Timestamp, field.Player, distanceToPiece, field.Piece );
		//public static ITaskField SetPiece( this ITaskField field, IFieldPiece piece = null ) => field.CreateTaskField( field.X, field.Y, field.Timestamp, field.Player, field.DistanceToPiece, piece );
	}
}