using Shared.Components.Pieces;

namespace Shared.Components.Players
{
	public interface IPlayer
	{
		ulong Id { get; }
		uint X { get; }
		uint Y { get; }
		IPiece Piece { get; }
	}
}
