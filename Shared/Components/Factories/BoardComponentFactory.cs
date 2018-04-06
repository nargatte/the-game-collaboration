using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Factories
{
	public class BoardComponentFactory : IBoardComponentFactory
	{
		#region IBoardComponentFactory
		public virtual ITaskField CreateTaskField( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece ) => new TaskField( x, y, timestamp, player, distanceToPiece, piece );
		public virtual IGoalField CreateGoalField( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type ) => new GoalField( x, y, team, timestamp, player, type );
		public virtual IFieldPiece CreateFieldPiece( ulong id, PieceType type, DateTime timestamp, ITaskField field ) => new FieldPiece( id, type, timestamp, field );
		public virtual IPlayerPiece CreatePlayerPiece( ulong id, PieceType type, DateTime timestamp, IPlayer player ) => new PlayerPiece( id, type, timestamp, player );
		public virtual IPlayer CreatePlayer( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece ) => new Player( id, team, type, timestamp, field, piece );		
		#endregion
	}
}
