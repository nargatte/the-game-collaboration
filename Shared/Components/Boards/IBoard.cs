using Shared.Components.Fields;
using Shared.Components.Players;

namespace Shared.Components.Boards
{
    public interface IBoard
    {
        uint Width { get; }
        uint Height { get; }
        uint TasksHeight { get; }
        uint GoalsHeight { get; }
        IField this[ uint x, uint y ] { get; }
		IPlayer GetPlayer( ulong id );
		void SetPlayer( ulong id, IPlayer player );
    }
}
