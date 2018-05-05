using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Components.Boards;

namespace Shared.Components.Factories
{
    public class BoardFactory : BoardComponentFactory, IBoardFactory
    {
        public IBoard CreateBoard( uint width, uint tasksHeight, uint goalsHeight ) => new Board( width, tasksHeight, goalsHeight, this );
    }
}
