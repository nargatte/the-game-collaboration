using Shared.Components.Players;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPlayer
	{
		public static void Clone( this IPlayer player, IPlayer aPlayer )
		{
			var field = player.Field;
			player.Field = null;
			var aField = field?.CloneField();
			player.Field = field;
			aPlayer.Field = aField;
			var piece = player.Piece;
			player.Piece = null;
			var aPiece = piece?.ClonePlayerPiece();
			player.Piece = piece;
			aPlayer.Piece = aPiece;
		}
		public static bool IsDefault( this IPlayer player ) => player.Timestamp == default && player.Field is null && player.Piece is null;
		public static uint? GetX( this IPlayer player ) => player.Field?.X;
		public static uint? GetY( this IPlayer player ) => player.Field?.Y;
	}
}