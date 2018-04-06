using Shared.Components.Fields;

namespace Shared.Components.Pieces
{
	public interface IFieldPiece : IPiece
	{
		ITaskField Field { get; set; }
		IFieldPiece CloneFieldPiece();
	}
}
