using Shared.Components.Events;
using Shared.Messages.Communication;
using System;

namespace GameMasterCore
{
    public interface IGameMaster
    {
        Data PerformMove(Move moveRequest);
        Data PerformDiscover(Discover discoverRequest);
        Data PerformPickUp(PickUpPiece pickUpRequest);
        Data PerformTestPiece(TestPiece testPieceRequest);
        Data PerformPlace(PlacePiece placeRequest);
        Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest);
        RegisteredGames PerformConfirmGameRegistration();
        PlayerMessage PerformJoinGame(JoinGame joinGame);
        event EventHandler<LogArgs> Log;

#warning TEMP, to change in future stages
        Game GetGame(string guid);
    }
}
