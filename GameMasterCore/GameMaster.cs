﻿using System;
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
using Config = Shared.Messages.Configuration;
using DTO = Shared.Messages.Communication;

namespace GameMasterCore
{
    public class GameMaster : IGameMaster
    {
        IBoard board;
        Dictionary<string, ulong> playerGuidToId;
        Config.GameMasterSettings config;

        public GameMaster()
        {
            this.config = new Config.GameMasterSettings
            {
                ActionCosts = new Config.GameMasterSettingsActionCosts(), //default ActionCosts
                GameDefinition = new Config.GameMasterSettingsGameDefinition(), //default GameDefinition, without Goals(!) and Name
            };
            this.board = PrapareBoard();
        }


        private IBoard PrapareBoard()
        {
            IBoard result = new Board(config.GameDefinition.BoardWidth, config.GameDefinition.TaskAreaLength, config.GameDefinition.GoalAreaLength);
            //set Goals from configuration
            foreach (var gf in config.GameDefinition.Goals)
                result.SetField(new GoalField(gf.x, gf.y, gf.team, type: GoalFieldType.Goal));
            //set the rest of GoalArea fields as NonGoals
            foreach (var f in result.Fields)
                if (f is GoalField gf && gf.Type == GoalFieldType.Unknown)
                    result.SetField(new GoalField(gf.X, gf.Y, gf.Team, type: GoalFieldType.NonGoal));
            //TODO: place players on the board
            //TODO: generate and place pieces
            return result;
        }

        public DTO.Data PerformDiscover(DTO.Discover discoverRequest)
        {
            //Find player and its on-board representation (aka pawn)
            var playerId = GetPlayerIdForGuid(discoverRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(playerId);
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
                playerId = playerId,
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
            ulong playerId = GetPlayerIdForGuid(moveRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(playerId);
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
            ulong playerId = GetPlayerIdForGuid(pickUpRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(playerId);
            //zwróć piece
            ITaskField field = (board.GetField(playerPawn.GetX().Value, playerPawn.GetY().Value) as TaskField);
            IPiece piece = field.Piece;

            if (piece == null)
            {
                return new DTO.Data
                {
                    playerId = playerPawn.Id
                };
            }

            //set player to contain the obtained piece
            //??? board.SetPlayer(new Player(playerPawn.Id, playerPawn.Team, playerPawn.Type, field:playerPawn.Field, piece: new PlayerPiece())

            //set field to not contain the piece anymore



            DTO.Data result = new DTO.Data
            {
                playerId = playerId,
                Pieces = new DTO.Piece[]
                {
                    new DTO.Piece
                    {
                        id = piece.Id,
                        playerId = playerId,
                        timestamp = DateTime.Now,
                        type = PieceType.Unknown
                    }
                }
            };

            return result;
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
            ulong playerId = GetPlayerIdForGuid(testPieceRequest.playerGuid);
            IPlayer playerPawn = board.GetPlayer(playerId);
            IPiece heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { playerId = playerId }; //player wanted to test inaccessible piece
            }

            //zwróć czy sham
            DTO.Data result = new DTO.Data
            {
                playerId = playerId,
                Pieces = new DTO.Piece[] {
                    new DTO.Piece
                    {
                        id = heldPiecePawn.Id,
                        timestamp = DateTime.Now,
                        playerId = playerId,
                        type = heldPiecePawn.Type
                    }
                }
            };
            return result;
        }

        private ulong GetPlayerIdForGuid(string guid) => playerGuidToId.FirstOrDefault(pair => pair.Key == guid).Value;

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
            
            fieldToReturn.distanceToPiece = (int)board.Pieces.
                Where(piece => piece is IFieldPiece).
                Select(piece => piece as IFieldPiece).
                Min(piece => Math.Abs(piece.Field.X - x) + Math.Abs(piece.Field.Y - y));

            pieces = piecesToReturn.ToArray();
            return fieldToReturn;
        }

        private DTO.GoalField GetGoalFieldInfo(int x, int y)
        {
            var relevantField = board.GetField((uint)x, (uint)y);
            var goalFieldToReturn = new DTO.GoalField
            {
                playerId = relevantField.Player?.Id ?? 0,
                playerIdSpecified = relevantField.Player != null,
                timestamp = relevantField.Timestamp,
                type = GoalFieldType.Unknown,
                team = y < board.GoalsHeight ? TeamColour.Blue : TeamColour.Red,
                x = (uint)x,
                y = (uint)y
            };

            return goalFieldToReturn;
        }
    }
}
