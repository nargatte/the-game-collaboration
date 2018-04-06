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
			var player = Player;
			Player = null;
			var aPlayer = player.ClonePlayer();
			Player = player;
			field.Player = aPlayer;
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
