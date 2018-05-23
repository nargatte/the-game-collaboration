using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Factories
{
	public interface IBoardComponentFactory
	{
		ITaskField CreateTaskField( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece );
		IGoalField CreateGoalField( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type );
		IFieldPiece CreateFieldPiece( ulong id, PieceType type, DateTime timestamp, ITaskField field );
		IPlayerPiece CreatePlayerPiece( ulong id, PieceType type, DateTime timestamp, IPlayer player );
		IPlayer CreatePlayer( ulong id, TeamColour team, PlayerRole type, DateTime timestamp, IField field, IPlayerPiece piece );
	}
}
