using Shared.Components.Fields;

namespace Shared.Components.Extensions
{
	public static class ExtensionITaskField
	{
		public static void Clone( this ITaskField field, ITaskField aField )
		{
			( field as IField ).Clone( aField );
			var piece = field.Piece;
			field.Piece = null;
			var aPiece = piece.CloneFieldPiece();
			field.Piece = piece;
			aField.Piece = aPiece;
		}
		public static bool IsDefault( this ITaskField field ) => ( field as IField ).IsDefault() && field.DistanceToPiece == -1 && field.Piece is null;
	}
}