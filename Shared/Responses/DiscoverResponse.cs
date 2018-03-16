using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Responses
{
    public class DiscoverResponse : GameMessage
    {
        List<TaskField> TaskFields;
        int? PlayerID;
        int? PieceID;
    }

    internal class TaskField : PositionalResponse
    {
        int DistanceToPiece;
    }
}
