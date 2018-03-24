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
using Shared.Enums;
using Shared.Messages.Configuration;
using DTO = Shared.Messages.Communication;

namespace GameMasterCore
{
    public class GameMaster : IGameMaster
    {
        IBoard board;
        Dictionary<string, ulong> playerGuidToId;
        GameMasterSettings config;

        public GameMaster()
        {
            config = new GameMasterSettings
            {
                ActionCosts = new GameMasterSettingsActionCosts(),
                GameDefinition = new GameMasterSettingsGameDefinition(), //default GameDefinition without Goals
            };

        }
        

        private void PrapareBoard()
        {
            board = new Board(
                config.GameDefinition.BoardWidth,
                config.GameDefinition.TaskAreaLength, 
                config.GameDefinition.GoalAreaLength
                );
        }

        public DTO.Data PerformDiscover(DTO.Discover discoverRequest)
        {
            //Find player and its on-board representation (aka pawn)
            IPlayer playerPawn = GetPlayerPawnFromGameMessage(discoverRequest);
            //Prepare result partial structures
            List<DTO.TaskField> resultFields = new List<DTO.TaskField>();
            List<DTO.Piece> resultPieces = new List<DTO.Piece>();
            //Perform discover itself on 3x3 
            for (int y = Math.Max((int)board.GoalsHeight, (int)playerPawn.GetY().Value - 1);
                y <= Math.Min(playerPawn.GetY().Value + 1, (int)board.Height - (int)board.GoalsHeight - 1);
                ++y)
                for (int x = Math.Max(0, (int)playerPawn.GetX().Value - 1);
                    x <= Math.Min(playerPawn.GetX().Value + 1, (int)board.Width - 1);
                    ++x)
                {
                    DTO.TaskField fieldToReturn = GetTaskFieldInfo(x, y, out DTO.Piece[] pieces);
                    resultFields.Add(fieldToReturn);
                    resultPieces.AddRange(pieces);
                }
            DTO.Data result = new DTO.Data
            {
                playerId = playerPawn.Id,
                Pieces = resultPieces.ToArray(),
                TaskFields = resultFields.ToArray(),
            };
            return result;
        }

        public DTO.Data PerformKnowledgeExchange(DTO.KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            throw new NotImplementedException();
        }

        public DTO.Data PerformMove(DTO.Move moveRequest)
        {
            //znajdź gracza po id
            IPlayer playerPawn = GetPlayerPawnFromGameMessage(moveRequest);
            //sprawdź czy może się ruszyć

            int targetX = (int)playerPawn.GetX().Value, targetY = (int)playerPawn.GetY().Value;
            switch (moveRequest.direction)
            {
                case MoveType.Down:
                    targetY--;
                    break;
                case MoveType.Left:
                    targetX--;
                    break;
                case MoveType.Right:
                    targetX++;
                    break;
                case MoveType.Up:
                    targetY++;
                    break;
                default:
                    break;
            }
            IField targetField = board.GetField((uint)targetX, (uint)targetY);
            if (targetField == null
                || (targetField is IGoalField gf && gf.Team != playerPawn.Team))
            {
                //do not move the player
                return new DTO.Data
                {
                    playerId = playerPawn.Id,
                    PlayerLocation = new DTO.Location { x = playerPawn.GetX().Value, y = playerPawn.GetY().Value }
                };
            }
            //rusz


            //zwróć informacje o obecnym polu i PlayerLocation
            throw new NotImplementedException();
        }

        public DTO.Data PerformPickUp(DTO.PickUpPiece pickUpRequest)
        {
            //znajdź playera po id w request
            //zwróć piece
            throw new NotImplementedException();
        }

        public DTO.Data PerformPlace(DTO.PlacePiece placeRequest)
        {
            //znajdź playera po id, znajdź jego piece
            //zobacz czy może odłożyć
            //zobacz czy zyskuje punkt
            //zobacz czy koniec gry
            throw new NotImplementedException();
        }

        public DTO.Data PerformTestPiece(DTO.TestPiece testPieceRequest)
        {
            //TODO znajdź playera po id, znajdź jego piece

            IPlayer playerPawn = GetPlayerPawnFromGameMessage(testPieceRequest);

            IPiece heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { playerId = playerPawn.Id }; //player wanted to test inaccessible piece
            }

            //zwróć czy sham
            DTO.Data result = new DTO.Data
            {
                playerId = playerPawn.Id,
                Pieces = new DTO.Piece[] {
                    new DTO.Piece
                    {
                        id = heldPiecePawn.Id,
                        timestamp = DateTime.Now,
                        playerId = playerPawn.Id,
                        type = heldPiecePawn.Type
                    }
                }
            };
            return result;
        }

        #region helperFunctions
        private ulong GetPlayerIdFromGuid(string guid) => playerGuidToId.FirstOrDefault(pair => pair.Key == guid).Value;

        private IPlayer GetPlayerPawnFromGameMessage(DTO.GameMessage gameMessage) => board.GetPlayer(GetPlayerIdFromGuid(gameMessage.playerGuid)); 
        #endregion

        private DTO.TaskField GetTaskFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            List<DTO.Piece> piecesToReturn = new List<DTO.Piece>();
            ITaskField currentField = board.GetField((uint)x, (uint)y) as ITaskField;
            DTO.TaskField fieldToReturn = new DTO.TaskField
            {
                x = (uint)x,
                y = (uint)y,
                timestamp = DateTime.Now,
            };
            if (currentField.Piece != null) //piece on the board
            {
                fieldToReturn.pieceId = currentField.Piece.Id;
                piecesToReturn.Add(new DTO.Piece
                {
                    id = currentField.Piece.Id,
                    type = PieceType.Unknown,
                    timestamp = DateTime.Now
                });
            }
            if (currentField.Player != null)
            {
                fieldToReturn.playerId = (ulong)currentField.Player.Id;
                if (board.GetPlayer((ulong)currentField.Player.Id).Piece != null) //check for held piece
                    piecesToReturn.Add(new DTO.Piece
                    {
                        id = board.GetPlayer((ulong)currentField.Player.Id).Piece.Id,
                        type = PieceType.Unknown,
                        timestamp = DateTime.Now,
                        playerId = (ulong)currentField.Player.Id
                    });

            }
            //TODO policz dystans do piece
            //fieldToReturn.distanceToPiece = ???

            pieces = piecesToReturn.ToArray();
            return fieldToReturn;
        }

        private DTO.GoalField GetGoalFieldInfo(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
