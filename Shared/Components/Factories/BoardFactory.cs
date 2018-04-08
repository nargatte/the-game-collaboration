using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Components.Boards;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;

namespace Shared.Components.Factories
{
    public class BoardFactory : BoardComponentFactory, IBoardFactory
    {
        public IBoard CreateBoard(uint width, uint tasksHeight, uint goalsHeight) => new Board(width, tasksHeight, goalsHeight, this);
    }
}
