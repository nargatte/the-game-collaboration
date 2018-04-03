using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Fields
{
	/// <summary>
	/// immutable
	/// </summary>
	public class GoalField : Field, IGoalField
	{
		#region Field
		public override IField CreateField( uint x, uint y, DateTime timestamp, IPlayer player ) => throw new NotSupportedException();
		#endregion
		#region IGoalField
		public virtual GoalFieldType Type { get; }
		public virtual TeamColour Team { get; }
		public virtual IGoalField CreateGoalField( uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type ) => new GoalField( x, y, team, timestamp, player, type );
		#endregion
		#region GoalField
		public GoalField( uint x, uint y, TeamColour team, DateTime timestamp = default, IPlayer player = null, GoalFieldType type = GoalFieldType.Unknown ) : base( x, y, timestamp, player )
		{
			Type = type;
			Team = team;
		}
		#endregion
	}
}
