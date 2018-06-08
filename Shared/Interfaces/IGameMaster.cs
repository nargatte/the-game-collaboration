using Shared.Components.Boards;
using Shared.Components.Events;
using Shared.DTO.Communication;
using System;

namespace Shared.Interfaces
{
    public interface IGameMaster
    {
        Data PerformMove(Move moveRequest);
        Data PerformDiscover(Discover discoverRequest);
        Data PerformPickUp(PickUpPiece pickUpRequest);
        Data PerformTestPiece(TestPiece testPieceRequest);
        Data PerformPlace(PlacePiece placeRequest);
        Data PerformDestroy(DestroyPiece destroyRequest);
        Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest);
        RegisteredGames PerformConfirmGameRegistration();
        Shared.DTO.Communication.PlayerMessage PerformJoinGame(Shared.DTO.Communication.JoinGame joinGame);
        event EventHandler<LogArgs> Log;
		IReadOnlyBoard Board { get; }
        Game GetGame(string guid);
    }
}
