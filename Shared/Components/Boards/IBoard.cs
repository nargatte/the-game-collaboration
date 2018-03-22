using Shared.Components.Fields;

namespace Shared.Components.Boards
{
    public interface IBoard
    {
        uint Width { get; }
        uint Height { get; }
        uint TasksHeight { get; }
        uint GoalsHeight { get; }
        IField this[ uint x, uint y ] { get; }
    }
}
