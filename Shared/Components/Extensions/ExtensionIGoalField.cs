using Shared.Components.Fields;
using Shared.Components.Players;
using Shared.Enums;
using System;

namespace Shared.Components.Extensions
{
	public static class ExtensionIGoalField
	{
		public static bool IsDefault( this IGoalField field ) => ( field as IField ).IsDefault() && field.Type == GoalFieldType.Unknown;
		public static IGoalField MakeGoalField( this IGoalField field, uint x, uint y, TeamColour team, DateTime timestamp = default, IPlayer player = null, GoalFieldType type = GoalFieldType.Unknown ) => field.CreateGoalField( x, y, team, timestamp, player, type );
		public static IGoalField SetTimestamp( this IGoalField field, DateTime timestamp = default ) => field.CreateGoalField( field.X, field.Y, field.Team, timestamp, field.Player, field.Type );
		public static IGoalField SetPlayer( this IGoalField field, IPlayer player = null ) => field.CreateGoalField( field.X, field.Y, field.Team, field.Timestamp, player, field.Type );
		public static IGoalField SetType( this IGoalField field, GoalFieldType type = GoalFieldType.Unknown ) => field.CreateGoalField( field.X, field.Y, field.Team, field.Timestamp, field.Player, type );
	}
}
