using Shared.Components.Boards;
using Shared.Components.Events;
using Shared.DTOs.Communication;
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
        Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest);
        RegisteredGames PerformConfirmGameRegistration();
        Shared.DTOs.Communication.PlayerMessage PerformJoinGame(Shared.DTOs.Communication.JoinGame joinGame);
        event EventHandler<LogArgs> Log;
		IReadOnlyBoard Board { get; }
        Game GetGame(string guid);
    }
}
