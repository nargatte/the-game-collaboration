using Shared.Components.Boards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.Factories
{
    public interface IBoardFactory : IBoardComponentFactory
    {
        IBoard CreateBoard(uint width, uint tasksHeight, uint goalsHeight);
    }
}
