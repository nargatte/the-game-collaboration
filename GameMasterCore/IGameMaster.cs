using Shared;
using Shared.Requests;
using Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMasterCore
{
    abstract public class IGameMaster
    {
        //IProxy proxy; //reference to CommunicationSubstitute
        IGame game;

        abstract protected IGame InitializeGame(IGameConfiguration gameConfiguration);
        abstract protected Data Move(MoveRequest moveRequest);
        abstract protected Data Discover(DiscoverRequest discoverRequest);
        abstract protected Data PickUp(PickUpRequest pickUpRequest);
        abstract protected Data TestPiece(TestPieceRequest testPieceRequest);
        abstract protected Data Place(PlaceRequest placeRequest);
        abstract protected Data KnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest);
    }
}
