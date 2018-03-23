using Shared.Enums;

namespace Shared.Components.Fields
{
    public interface IGoalField : IField
    {
        GoalFieldType Type { get; }
        TeamColour Team { get; }
    }
}
