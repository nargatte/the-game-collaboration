using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Data
    {
        ulong playerId;
        List<TaskField> TaskFields;
        List<GoalField> GoalFields;
        List<Piece> Pieces;
        Location PlayerLocation;
        bool gameFinished;
    }

    internal class Location
    {
        uint x;
        uint y;
    }
}
