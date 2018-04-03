namespace Shared.Components.Players
{
	public static class ExtenderIPlayer
	{
		public static uint? GetX( this IPlayer player ) => player?.Field.X;
		public static uint? GetY( this IPlayer player ) => player?.Field.Y;
	}
}
