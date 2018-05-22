using System;
using Shared.Components.Factories;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using Shared.Components.Boards;

namespace PlayerCoreUnitTests
{
    public partial class StateTests
    {
        class MockBoardFactory : IBoardFactory
        {
            public IBoard CreateBoard(uint width, uint tasksHeight, uint goalsHeight) => throw new NotImplementedException();

            public IFieldPiece CreateFieldPiece(ulong id, PieceType type, DateTime timestamp, ITaskField field) => throw new NotImplementedException();

            public IGoalField CreateGoalField(uint x, uint y, TeamColour team, DateTime timestamp, IPlayer player, GoalFieldType type) => throw new NotImplementedException();

            public IPlayer CreatePlayer(ulong id, TeamColour team, PlayerRole type, DateTime timestamp, IField field, IPlayerPiece piece) => throw new NotImplementedException();

            public IPlayerPiece CreatePlayerPiece(ulong id, PieceType type, DateTime timestamp, IPlayer player) => throw new NotImplementedException();

            public ITaskField CreateTaskField(uint x, uint y, DateTime timestamp, IPlayer player, int distanceToPiece, IFieldPiece piece) => throw new NotImplementedException();
        }

    }

}
