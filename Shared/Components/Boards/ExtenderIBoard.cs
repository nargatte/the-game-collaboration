using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.Boards
{
    public static class ExtenderIBoard
    {
        public static IField GetField(this IBoard board, Location location) => board.GetField(location.x, location.y);

        public static bool SetPlayer(this IBoard board, ulong id, Location location, DateTime timestamp) =>
            board.SetPlayer(new Players.Player(board.GetPlayer(id), timestamp, board.GetField(location)));
    }
}
