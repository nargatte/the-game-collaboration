using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;

namespace Shared.Components.Factories
{
	public interface IBoardPrototypeFactory
	{
		ITaskField TaskField { get; }
		IGoalField GoalField { get; }
		IFieldPiece FieldPiece { get; }
		IPlayerPiece PlayerPiece { get; }
		IPlayer Player { get; }
	}
}
