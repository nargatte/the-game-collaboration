using NUnit.Framework;
using PlayerCore;
using Shared.Messages.Communication;

namespace PlayerCoreUnitTests
{
    [TestFixture]
    public class StateTests
    {
        [Test]
        public void ReceiveData_DataEmpty_NoChange()
        {
            State state = GetState();
            Data data = GetData();

            state.ReceiveData(data);

            Assert.Multiple(() =>
            {
                Assert.IsFalse(state.HasPiece);
                Assert.AreEqual(0, state.Game.Players.Length);
                Assert.AreEqual(0, state.Game.PlayerLocation.x);
                Assert.AreEqual(0, state.Game.PlayerLocation.y);
                Assert.AreEqual(0, state.Game.Board.width);
                Assert.AreEqual(0, state.Game.Board.tasksHeight);
                Assert.AreEqual(0, state.Game.Board.goalsHeight);
            });
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(3, 60)]
        [TestCase(5, 2)]
        [TestCase(0, 0)]
        public void ReceiveData_NewLocation_LocationChange(uint x, uint y)
        {
            State state = GetState();
            Data data = GetData();
            data.PlayerLocation.x = x;
            data.PlayerLocation.y = y;

            state.ReceiveData(data);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(x, state.Game.PlayerLocation.x);
                Assert.AreEqual(y, state.Game.PlayerLocation.y);
            });
        }

        public void ReceiveData_EndingGame_InvokeEvent()
        {
            State state = GetState();
            Data data = GetData();
            data.gameFinished = true;
            bool wasCalled = false;
            state.EndGame += (s, a) => wasCalled = true;

            state.ReceiveData(data);

            Assert.IsTrue(wasCalled);
        }

        public void ReceiveData_Descover_UpDateBoard()
        {
            State state = GetState();
            Data data = GetData();
        }

        private State GetState() => new State();

        private Data GetData()
        {
            Data data = new Data();
            data.TaskFields = new TaskField[0];
            data.GoalFields = new GoalField[0];
            data.Pieces = new Piece[0];
            data.PlayerLocation = new Location();
            return data;
        }

        private Piece GetPiece()
        {
            Piece piece = new Piece();
            piece.type = PieceType.unknown;
            piece.timestamp = new System.DateTime(0);
            return piece;
        }
    }
}
