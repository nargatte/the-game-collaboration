using Shared.Components.Boards;

namespace Shared.Components.Factories
{
    public interface IBoardFactory : IBoardComponentFactory
    {
        IBoard CreateBoard( uint width, uint tasksHeight, uint goalsHeight );
    }
}
