using Shared.Enums;

namespace Shared.Components.Fields
{
    interface IGoalField : IField
    {
        GoalFieldType Type { get; }
        TeamColour Team { get; }
    }
}
