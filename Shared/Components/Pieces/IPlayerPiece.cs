using Shared.Components.Players;

namespace Shared.Components.Pieces
{
	public interface IPlayerPiece : IPiece
	{
		IPlayer Player { get; }
	}
}
