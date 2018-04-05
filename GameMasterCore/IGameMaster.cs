using Shared;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMasterCore
{
    public interface IGameMaster
    {
        //IProxy proxy; //reference to CommunicationSubstitute
        //IGame game;

        //IGame InitializeGame(IGameConfiguration gameConfiguration);
        Data PerformMove(Move moveRequest);
        Data PerformDiscover(Discover discoverRequest);
        Data PerformPickUp(PickUpPiece pickUpRequest);
        Data PerformTestPiece(TestPiece testPieceRequest);
        Data PerformPlace(PlacePiece placeRequest);
        Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest);
        Data PerformConfirmGameRegistration(RegisteredGames registeredGames);
        PlayerMessage PerformJoinGame(JoinGame joinGame);
    }
}
