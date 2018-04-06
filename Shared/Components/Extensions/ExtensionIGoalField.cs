using Shared.Components.Fields;
using Shared.Enums;

namespace Shared.Components.Extensions
{
	public static class ExtensionIGoalField
	{
		public static void Clone( this IGoalField field, IGoalField aField ) => ( field as IField ).Clone( aField );
		public static bool IsDefault( this IGoalField field ) => ( field as IField ).IsDefault() && field.Type == GoalFieldType.Unknown;
	}
}
