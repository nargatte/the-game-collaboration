using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Components.Boards;
using Shared.Components.Fields;
using Shared.Components.Players;
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
            //Find player and its on-board representation (aka pawn)
            Player player = GetPlayerForGuid(discoverRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(player.id);
            //Prepare result partial structures
            List<TaskField> resultFields = new List<TaskField>();
            List<Piece> resultPieces = new List<Piece>();
            //Perform discover itself on 3x3 
            for (int y = Math.Max((int)board.GoalsHeight, (int)playerPawn.Y - 1); y <= Math.Min(playerPawn.X + 1, board.Height-board.GoalsHeight-1); ++x)
                for (int x = Math.Max(0, (int)playerPawn.X - 1);x <= Math.Min(playerPawn.X+1,board.Width-1); ++x)
                {
                    ITaskField currentField = board[(uint)x, (uint)y] as ITaskField;//current field is a task field because of for-loops' bounds
                    TaskField current = new TaskField
                    {
                        x = (uint)x,
                        y = (uint)y,
                        timestamp = DateTime.Now,
                    };
                    if (currentField.Piece != null) //piece on the board
                    {
                        current.pieceId = currentField.Piece.Id;
                        resultPieces.Add(new Piece
                        {
                            id = currentField.Piece.Id,
                            type = currentField.Piece.Type,
                            timestamp = DateTime.Now
                        });
                    }
                    if (currentField.PlayerId != null)
                    {
                        current.playerId = (ulong)currentField.PlayerId;
                        if(board.GetPlayer((ulong)currentField.PlayerId).Piece != null) //check for a held piece
                            resultPieces.Add(new Piece
                            {
                                id = board.GetPlayer((ulong)currentField.PlayerId).Piece.Id,
                                type = board.GetPlayer((ulong)currentField.PlayerId).Piece.Type,
                                timestamp = DateTime.Now,
                                playerId = (ulong)currentField.PlayerId
                            });
                        
                    }
                    //TODO policz dystans do piece
                    //result.distanceToPiece = ???

                    resultFields.Add(current);
                }
            Data result = new Data
            {
                playerId = player.id,
                Pieces = resultPieces.ToArray(),
                TaskFields = resultFields.ToArray(),
            };
            return result;
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
