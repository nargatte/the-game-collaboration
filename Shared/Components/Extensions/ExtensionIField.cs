using Shared.Components.Fields;

namespace Shared.Components.Extensions
{
	public static class ExtensionIField
	{
		public static void Clone( this IField field, IField aField )
		{
			var player = field.Player;
			field.Player = null;
			var aPlayer = player?.ClonePlayer();
			field.Player = player;
			aField.Player = aPlayer;
		}
		public static bool IsDefault( this IField field ) => field.Timestamp == default && field.Player is null;
	}
}
