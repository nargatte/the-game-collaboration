using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IGameActions
    {
        void Move(Move move);

        void Discover(Discover discover);

        void PickUpPiece(PickUpPiece pickUpPiece);

        void TestPiece(TestPiece testPiece);

        void PlacePiece(PlacePiece placePiece);

        void Communicate(AuthorizeKnowledgeExchange authorizeKnowledgeExchange);
    }
}
