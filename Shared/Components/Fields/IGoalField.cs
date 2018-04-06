using Shared.Enums;

namespace Shared.Components.Fields
{
	public interface IGoalField : IField
    {
        GoalFieldType Type { get; set; }
        TeamColour Team { get; set; }
		IGoalField CloneGoalField();
	}
}
