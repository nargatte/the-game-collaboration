using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Fields
{
    public interface IGoalField : IField
    {
        GoalFieldType Type { get; }
        TeamColour Team { get; }
		IGoalField CreateGoalField( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type );
    }
}
