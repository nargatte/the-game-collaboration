using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace Shared.Components.Factories
{
	/// <summary>
	/// robust
	/// </summary>
	public class BoardPrototypeFactory : IBoardPrototypeFactory
	{
		#region IBoardPrototypeFactory
		public virtual ITaskField TaskField { get; } = new TaskField( default, default );
		public virtual IGoalField GoalField { get; } = new GoalField( default, default, default );
		public virtual IFieldPiece FieldPiece { get; } = new FieldPiece( default );
		public virtual IPlayerPiece PlayerPiece { get; } = new PlayerPiece( default );
		public virtual IPlayer Player { get; } = new Player( default, default, default );
		#endregion
	}
}
