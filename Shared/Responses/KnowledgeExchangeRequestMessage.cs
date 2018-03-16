using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Responses
{
    public class KnowledgeExchangeRequestMessage : GameMessage
    {
        List<Piece> Pieces;
        List<GoalField> GoalFields;
        List<TaskField> TaskFields;
    }

    internal class GoalField
    {
        string Team;
        enum GoalFieldType { goal, nongoal };
        GoalFieldType Type;
    }

    internal class Piece
    {
        int ID;
        DateTime TimeStamp;
        enum PieceType { sham, unknown };
        PieceType pieceType;
    }
}
