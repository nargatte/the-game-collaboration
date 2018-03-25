using System;
using Shared.Components.Players;
using Shared.Enums;

namespace Shared.Components.Fields
{
	/// <summary>
	/// immutable
	/// </summary>
	public class GoalField : Field, IGoalField
	{
		#region IGoalField
		public virtual GoalFieldType Type { get; }
		public virtual TeamColour Team { get; }
		#endregion
		#region GoalField
		public GoalField( uint x, uint y, TeamColour team, DateTime timestamp = default( DateTime ), IPlayer player = null, GoalFieldType type = GoalFieldType.Unknown ) : base( x, y, timestamp, player )
		{
			Type = type;
			Team = team;
		}

		public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => throw new NotImplementedException();
		#endregion
	}
}
