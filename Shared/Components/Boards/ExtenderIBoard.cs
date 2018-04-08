using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.Boards
{
    public static class ExtenderIBoard
    {
        public static IField GetField(this IBoard board, Location location) => board.GetField(location.x, location.y);

        public static void SetPlayerLocation(this IBoard board, ulong id,  Location location, DateTime dataTime)
        { 
            var player = board.GetPlayer(id);
            var field = board.GetField(location);
            player.Field = field ?? throw new Exception($"Field in {location.x} {location.y} is null");
            if(player.Field == null && field != null)
                throw new Exception("Field in player is null after assinging not-null");
            board.SetPlayer(player);
            Console.WriteLine($"SetPlayerLocation id={id}, new location {location.x} {location.y}");
        }
    }
}
