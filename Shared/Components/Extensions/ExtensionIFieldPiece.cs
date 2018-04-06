using Shared.Components.Pieces;

namespace Shared.Components.Extensions
{
	public static class ExtensionIFieldPiece
	{
		public static void Clone( this IFieldPiece piece, IFieldPiece aPiece )
		{
			( piece as IPiece ).Clone( aPiece );
			var field = piece.Field;
			piece.Field = null;
			var aField = field.CloneTaskField();
			piece.Field = field;
			aPiece.Field = aField;
		}
		public static bool IsDefault( this IFieldPiece piece ) => ( piece as IPiece ).IsDefault() && piece.Field is null;
	}
}