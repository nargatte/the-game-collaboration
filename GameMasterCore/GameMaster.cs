using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Components.Boards;
using Shared.Messages.Communication;

namespace GameMasterCore
{
    class GameMaster : IGameMaster
    {
        //board, players (z kolorami)
        IBoard board;
        Dictionary<string, Player> guidToPlayer; //lub string->ulong

        public GameMaster()
        {

        }

        public Data PerformDiscover(Discover discoverRequest)
        {
            //znajdź playera po id w request
            Player player = GetPlayerForGuid(discoverRequest.playerGuid);
            //zrób discover
            throw new NotImplementedException();
        }

        public Data PerformKnowledgeExchange(KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            throw new NotImplementedException();
        }

        public Data PerformMove(Move moveRequest)
        {
            //znajdź gracza po id
            //sprawdź czy może się ruszyć
            //rusz
            throw new NotImplementedException();
        }

        public Data PerformPickUp(PickUpPiece pickUpRequest)
        {
            //znajdź playera po id w request
            //zwróć piece
            throw new NotImplementedException();
        }

        public Data PerformPlace(PlacePiece placeRequest)
        {
            //znajdź playera po id, znajdź jego piece
            //zobacz czy może odłożyć
            //zobacz czy zyskuje punkt
            //zobacz czy koniec gry
            throw new NotImplementedException();
        }

        public Data PerformTestPiece(TestPiece testPieceRequest)
        {
            //TODO znajdź playera po id, znajdź jego piece
            Player player = GetPlayerForGuid(testPieceRequest.playerGuid);
            Piece heldPiece;
            
            //zwróć czy sham
            Data result = new Data
            {
                Pieces = new Piece[] {
                    new Piece
                    {
                        id = heldPiece.id,
                        timestamp = DateTime.Now,
                        playerId = player.id,
                        type = heldPiece.type
                    }
                }
            };
            return result;
        }

        private Player GetPlayerForGuid(string guid) => guidToPlayer.FirstOrDefault(pair => pair.Key == guid).Value;
    }
}
