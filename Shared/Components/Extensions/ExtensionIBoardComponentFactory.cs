using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIBoardComponentFactory
	{
		public static ITaskField MakeTaskField( this IBoardComponentFactory factory, uint x, uint y, DateTime timestamp = default, IPlayer player = null, int distanceToPiece = -1, IFieldPiece piece = null ) => factory.CreateTaskField( x, y, timestamp, player, distanceToPiece, piece );
		public static IGoalField MakeGoalField( this IBoardComponentFactory factory, uint x, uint y, TeamColour team, DateTime timestamp = default, IPlayer player = null, GoalFieldType type = GoalFieldType.Unknown ) => factory.CreateGoalField( x, y, team, timestamp, player, type );
		public static IFieldPiece MakeFieldPiece( this IBoardComponentFactory factory, ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, ITaskField field = null ) => factory.CreateFieldPiece( id, type, timestamp, field );
		public static IPlayerPiece MakePlayerPiece( this IBoardComponentFactory factory, ulong id, PieceType type = PieceType.Unknown, DateTime timestamp = default, IPlayer player = null ) => factory.CreatePlayerPiece( id, type, timestamp, player );
		public static IPlayer MakePlayer( this IBoardComponentFactory factory, ulong id, TeamColour team, PlayerRole type, DateTime timestamp = default, IField field = null, IPlayerPiece piece = null ) => factory.CreatePlayer( id, team, type, timestamp, field, piece );
	}
}
