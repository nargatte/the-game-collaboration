using Shared.Components.Extensions;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Fields
{
	public class GoalField : Field, IGoalField
	{
		#region Field
		public override IField CloneField() => CloneGoalField();
		#endregion
		#region IGoalField
		public virtual GoalFieldType Type { get; set; }
		public virtual TeamColour Team { get; set; }
		public virtual IGoalField CloneGoalField()
		{
			var field = new GoalField( X, Y, Team, Timestamp, null, Type );
			this.Clone( field );
			return field;
		}
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
