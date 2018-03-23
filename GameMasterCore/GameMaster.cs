using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Components.Boards;
using Shared.Components.Fields;
using Shared.Components.Pieces;
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
            for (int y = Math.Max((int)board.GoalsHeight, (int)playerPawn.Y - 1);
                y <= Math.Min(playerPawn.Y + 1, board.Height - board.GoalsHeight - 1);
                ++y)
                for (int x = Math.Max(0, (int)playerPawn.X - 1);
                    x <= Math.Min(playerPawn.X + 1, board.Width - 1);
                    ++x)
                {
                    TaskField fieldToReturn = GetFieldInfo(x, y, out Piece[] pieces);
                    resultFields.Add(fieldToReturn);
                    resultPieces.AddRange(pieces);
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
            Player player = GetPlayerForGuid(moveRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(player.id);
            //sprawdź czy może się ruszyć

            //rusz
            //zwróć informacje o obecnym polu i PlayerLocation
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
            IPlayer playerPawn = board.GetPlayer(player.id);
            IPiece heldPiecePawn = playerPawn.Piece;
            if(heldPiecePawn == null)
            {
                return new Data { playerId = player.id }; //player wanted to test inaccessible piece
            }

            //zwróć czy sham
            Data result = new Data
            {
                playerId = player.id,
                Pieces = new Piece[] {
                    new Piece
                    {
                        id = heldPiecePawn.Id,
                        timestamp = DateTime.Now,
                        playerId = player.id,
                        type = heldPiecePawn.Type
                    }
                }
            };
            return result;
        }

        private Player GetPlayerForGuid(string guid) => guidToPlayer.FirstOrDefault(pair => pair.Key == guid).Value;

        private TaskField GetFieldInfo(int x, int y, out Piece[] pieces)
        {
            List<Piece> piecesToReturn = new List<Piece>();
            ITaskField currentField = board[(uint)x, (uint)y] as ITaskField;//current field is a task field because of for-loops' bounds
            TaskField fieldToReturn = new TaskField
            {
                x = (uint)x,
                y = (uint)y,
                timestamp = DateTime.Now,
            };
            if (currentField.Piece != null) //piece on the board
            {
                fieldToReturn.pieceId = currentField.Piece.Id;
                piecesToReturn.Add(new Piece
                {
                    id = currentField.Piece.Id,
                    type = PieceType.unknown,
                    timestamp = DateTime.Now
                });
            }
            if (currentField.PlayerId != null)
            {
                fieldToReturn.playerId = (ulong)currentField.PlayerId;
                if (board.GetPlayer((ulong)currentField.PlayerId).Piece != null) //check for held piece
                    piecesToReturn.Add(new Piece
                    {
                        id = board.GetPlayer((ulong)currentField.PlayerId).Piece.Id,
                        type = PieceType.unknown,
                        timestamp = DateTime.Now,
                        playerId = (ulong)currentField.PlayerId
                    });

            }
            //TODO policz dystans do piece
            //fieldToReturn.distanceToPiece = ???

            pieces = piecesToReturn.ToArray();
            return fieldToReturn;
        }
    }
}
