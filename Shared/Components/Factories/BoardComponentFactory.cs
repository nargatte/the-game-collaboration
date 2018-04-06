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
		public virtual ITaskField CreateTaskField( uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece ) => throw new NotImplementedException();
		public virtual IGoalField CreateGoalField( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type ) => throw new NotImplementedException();
		public virtual IFieldPiece CreateFieldPiece( ulong id, PieceType type, DateTime timestamp, ITaskField field ) => throw new NotImplementedException();
		public virtual IPlayerPiece CreatePlayerPiece( ulong id, PieceType type, DateTime timestamp, IPlayer player ) => throw new NotImplementedException();
		public virtual IPlayer CreatePlayer( ulong id, TeamColour team, PlayerType type, DateTime timestamp, IField field, IPlayerPiece piece ) => throw new NotImplementedException();		
		#endregion
	}
}
