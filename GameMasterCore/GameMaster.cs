using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Requests;
using Shared.Responses;

namespace GameMasterCore
{
    class GameMaster : IGameMaster
    {
        protected override Data Discover(DiscoverRequest discoverRequest)
        {
            //znajdź playera po id w request
            //zrób discover
            throw new NotImplementedException();
        }

        protected override Data KnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            throw new NotImplementedException();
        }

        protected override Data Move(MoveRequest moveRequest)
        {
            //znajdź gracza po id
            //sprawdź czy może się ruszyć
            //rusz
            throw new NotImplementedException();
        }

        protected override Data PickUp(PickUpRequest pickUpRequest)
        {
            //znajdź playera po id w request
            //zwróć piece
            throw new NotImplementedException();
        }

        protected override Data Place(PlaceRequest placeRequest)
        {
            //znajdź playera po id, znajdź jego piece
            //zobacz czy może odłożyć
            //zobacz czy zyskuje punkt
            //zobacz czy koniec gry
            throw new NotImplementedException();
        }

        protected override Data TestPiece(TestPieceRequest testPieceRequest)
        {
            //znajdź playera po id, znajdź jego piece
            //zwróć czy sham
            throw new NotImplementedException();
        }
    }
}
