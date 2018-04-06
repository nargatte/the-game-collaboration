using Shared.Components.Pieces;
using Shared.Enums;

namespace Shared.Components.Extensions
{
	public static class ExtensionIPiece
	{
		public static void Clone( this IPiece piece, IPiece aPiece )
		{
		}
		public static bool IsDefault( this IPiece piece ) => piece.Type == PieceType.Unknown && piece.Timestamp == default;
	}
}