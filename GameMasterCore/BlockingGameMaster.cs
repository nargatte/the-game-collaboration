using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.Components.Boards;
using Shared.Components.Factories;
using Shared.Components.Extensions;
using Shared.Components.Fields;
using Shared.Components.Pieces;
using Shared.Components.Players;
using Shared.Enums;
using Config = Shared.DTOs.Configuration;
using DTO = Shared.DTOs.Communication;
using System.Threading;
using Shared.Components.Events;
using Shared.Interfaces;

namespace GameMasterCore
{
    public class BlockingGameMaster : IGameMaster
    {
        Random random;
        public virtual IReadOnlyBoard Board => board;
        public IBoard board;
        public Dictionary<string, ulong> playerGuidToId;
        public Dictionary<ulong, DTO.GameMessage> playerBusy = new Dictionary<ulong, DTO.GameMessage>();
        int playerIDcounter = 0;
        ulong pieceIDcounter = 0;
        Config.GameMasterSettings config;
        int redGoalsToScore, blueGoalsToScore;
        public Dictionary<ulong, DTO.Game> Game { get; set; } // for process game by communication substitute 
        public ulong gameId;

        public BlockingGameMaster(int seed = 123456)
        {
            playerGuidToId = new Dictionary<string, ulong>();
            random = new Random(seed);

            // prepare default config
            config = GenerateDefaultConfig();

            // generate board itself from config
            board = PrepareBoard(new BoardComponentFactory());
        }

        public BlockingGameMaster(Config.GameMasterSettings _config, IBoardComponentFactory _boardComponentFactory, int seed = 123456)
        {
            playerGuidToId = new Dictionary<string, ulong>();
            random = new Random(seed);

            config = _config;
            board = PrepareBoard(_boardComponentFactory);
        }

        #region Preparation
        private Config.GameMasterSettings GenerateDefaultConfig()
        {
            var result = new Config.GameMasterSettings
            {
                ActionCosts = new Config.GameMasterSettingsActionCosts(), // default ActionCosts
                GameDefinition = new Config.GameMasterSettingsGameDefinition()
                {
                    GameName = "default game"
                }, //default GameDefinition, without Goals(!) and Name
            };

            //generate Goals for default config without goals
            var goalLocationsBlue = GenerateRandomPlaces(6, 0,
                result.GameDefinition.BoardWidth, 0,
                result.GameDefinition.GoalAreaLength
                );
            var goalLocationsRed = goalLocationsBlue.Select(location => new DTO.Location()
            {
                X = location.X,
                Y = 2 * result.GameDefinition.GoalAreaLength + result.GameDefinition.TaskAreaLength - 1 - location.Y
            }
            ).ToList();

            result.GameDefinition.Goals = goalLocationsBlue.Select(location =>
                new Config.GoalField
                {
                    Team = TeamColour.Blue,
                    Type = GoalFieldType.Goal,
                    X = location.X,
                    Y = location.Y
                }
            ).Concat(goalLocationsRed.Select(location =>
                new Config.GoalField
                {
                    Team = TeamColour.Red,
                    Type = GoalFieldType.Goal,
                    X = location.X,
                    Y = location.Y
                }
            )).ToList();

            return result;
        }

        private IBoard PrepareBoard(IBoardComponentFactory boardComponentFactory)
        {
            IBoard result = new Board(
                config.GameDefinition.BoardWidth,
                config.GameDefinition.TaskAreaLength,
                config.GameDefinition.GoalAreaLength,
                boardComponentFactory
                );
            //set Goals from configuration
            foreach (var gf in config.GameDefinition.Goals)
            {
                if (gf.Team == TeamColour.Red)
                    redGoalsToScore++;
                else
                    blueGoalsToScore++;
                result.SetField(
                    result.Factory.CreateGoalField(gf.X, gf.Y, gf.Team, DateTime.Now, null, GoalFieldType.Goal)
                    );
            }
            //set the rest of GoalArea fields as NonGoals
            foreach (var f in result.Fields)
                if (f is IGoalField gf && gf.Type == GoalFieldType.Unknown)
                    result.SetField(
                        result.Factory.CreateGoalField(gf.X, gf.Y, gf.Team, DateTime.Now, null, GoalFieldType.NonGoal)
                        );

            GenerateRandomPlaces(
                config.GameDefinition.InitialNumberOfPieces,
                0, result.Width, result.GoalsHeight, result.Height - result.GoalsHeight).ForEach(
                    place => result.SetPiece(result.Factory.CreateFieldPiece(++pieceIDcounter, GetRandomPieceType(), DateTime.Now, (ITaskField)result.GetField(place)))
                );

            return result;
        }
        #endregion

        #region Synced helper methods for IGameMaster
        private DTO.Data PerformSynchronizedDiscover(DTO.Discover discoverRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(discoverRequest);

            OnLog("discover", DateTime.Now, gameId, playerPawn.Id, discoverRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            //Prepare partial result structures
            var resultFields = new List<DTO.TaskField>();
            var resultPieces = new List<DTO.Piece>();
            //Perform discover on 3x3 
            for (
                int y = Math.Max(
                    (int)board.GoalsHeight,
                    (int)playerPawn.GetY().Value - 1
                    );
                y <= Math.Min(
                    playerPawn.GetY().Value + 1,
                    (int)board.Height - (int)board.GoalsHeight - 1
                    );
                ++y
                )
                for (
                    int x = Math.Max(
                        0,
                        (int)playerPawn.GetX().Value - 1
                        );
                    x <= Math.Min(
                        playerPawn.GetX().Value + 1,
                        (int)board.Width - 1
                        );
                    ++x
                    )
                {
                    var fieldToReturn = GetTaskFieldInfo(x, y, out var pieces);
                    resultFields.Add(fieldToReturn);
                    resultPieces.AddRange(pieces);
                }
            var result = new DTO.Data
            {
                PlayerId = playerPawn.Id,
                Pieces = resultPieces.ToList(),
                TaskFields = resultFields.ToList(),
                PlayerLocation = new DTO.Location { X = playerPawn.GetX().Value, Y = playerPawn.GetY().Value }
            };
            return result;
        }

		private DTO.Data PerformSynchronizedKnowledgeExchange( DTO.KnowledgeExchangeRequest knowledgeExchangeRequest ) =>
			//TODO: knowledge exchange
			throw new NotImplementedException();

		private DTO.Data PerformSynchronizedMove(DTO.Move moveRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(moveRequest);

            OnLog("move", DateTime.Now, gameId, playerPawn.Id, moveRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            int targetX = (int)playerPawn.GetX().Value, targetY = (int)playerPawn.GetY().Value;
            switch (moveRequest.Direction)
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

            var targetField = targetX < 0 || targetX >= board.Width || targetY < 0 || targetY >= board.Height
                ? null : board.GetField((uint)targetX, (uint)targetY);
            //check for invalid moves
            if (targetField == null
                || (targetField is IGoalField gf && gf.Team != playerPawn.Team))
            {
                //trying to move outside the board or to other team's goal area - do not move the player
                return new DTO.Data
                {
                    PlayerId = playerPawn.Id,
                    PlayerLocation = new DTO.Location { X = playerPawn.GetX().Value, Y = playerPawn.GetY().Value }
                };
            }
            if (targetField.Player != null)
            {
                //field is occupied - do not move the player, return info about the occupied field
                var occupiedField = GetFieldInfo(targetX, targetY, out var pieces);
                var occupiedResult = new DTO.Data
                {
                    PlayerId = playerPawn.Id,
                    PlayerLocation = new DTO.Location { X = playerPawn.GetX().Value, Y = playerPawn.GetY().Value },
                    Pieces = pieces.ToList(),
                    GoalFields = (occupiedField is DTO.GoalField) ? new List<DTO.GoalField> { occupiedField as DTO.GoalField } : null,
                    TaskFields = (occupiedField is DTO.TaskField) ? new List<DTO.TaskField> { occupiedField as DTO.TaskField } : null
                };
                return occupiedResult;
            }

            //move
            board.SetPlayer(board.Factory.CreatePlayer(
                playerPawn.Id,
                playerPawn.Team,
                playerPawn.Type,
                DateTime.Now,
                targetField,
                playerPawn.Piece
                ));

            //return information about current field and new player location
            var currentField = GetFieldInfo(targetX, targetY, out var currentPieces);

            var result = new DTO.Data
            {
                PlayerId = playerPawn.Id,
                PlayerLocation = new DTO.Location { X = (uint)targetX, Y = (uint)targetY },
                Pieces = (currentPieces == null) ? new List<DTO.Piece>() : currentPieces.ToList(),
                GoalFields = (currentField is DTO.GoalField) ? new List<DTO.GoalField> { currentField as DTO.GoalField } : new List<DTO.GoalField>(),
                TaskFields = (currentField is DTO.TaskField) ? new List<DTO.TaskField> { currentField as DTO.TaskField } : new List<DTO.TaskField>()
            };
            return result;
        }

        private DTO.Data PerformSynchronizedPickUp(DTO.PickUpPiece pickUpRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(pickUpRequest);

            OnLog("pickup", DateTime.Now, gameId, playerPawn.Id, pickUpRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            var field = (board.GetField(playerPawn.GetX().Value, playerPawn.GetY().Value) as ITaskField);
            IPiece piece = field.Piece;

            if (piece == null)
            {
                return new DTO.Data
                {
                    PlayerId = playerPawn.Id
                };
            }

            board.SetPiece(board.Factory.CreatePlayerPiece(piece.Id, piece.Type, DateTime.Now, playerPawn));

            //prepare result data
            var result = new DTO.Data
            {
                PlayerId = playerPawn.Id,
                Pieces = new List<DTO.Piece>
                {
                    new DTO.Piece
                    {
                        Id = piece.Id,
                        PlayerId = playerPawn.Id,
                        Timestamp = DateTime.Now,
                        Type = PieceType.Unknown
                    }
                }
            };
            return result;
        }

        public void PerformCreatePieceAndPlaceRandomly()
        {
            lock (board)
            {
                DTO.Location place;
                do
                {
                    place = GenerateRandomPlaces(1, 0, board.Width, board.GoalsHeight, board.Height - board.GoalsHeight - 1).First();
                } while (board.GetField(place.X, place.Y).Player != null);

                var field = board.GetField(place.X, place.Y);
                var newPiece = board.Factory.CreateFieldPiece(++pieceIDcounter, GetRandomPieceType(), DateTime.Now, (ITaskField)field);
                board.SetPiece(newPiece);
            }
        }

        private DTO.Data PerformSynchronizedDestroy(DTO.DestroyPiece destroyRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(destroyRequest);

            OnLog("destroy", DateTime.Now, gameId, playerPawn.Id, destroyRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            var heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { PlayerId = playerPawn.Id }; //player wanted to destroy inaccessible piece
            }

            board.SetPiece(board.Factory.CreatePlayerPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, null));
            return new DTO.Data {
                PlayerId = playerPawn.Id,
                PlayerLocation = new DTO.Location { X = playerPawn.GetX().Value, Y = playerPawn.GetY().Value }
            };
        }

        private DTO.Data PerformSynchronizedPlace(DTO.PlacePiece placeRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(placeRequest);

            OnLog("place", DateTime.Now, gameId, playerPawn.Id, placeRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            var heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { PlayerId = playerPawn.Id }; //player wanted to place inaccessible piece
            }
            var targetField = playerPawn.Field;
            if (targetField is ITaskField targetTaskField)
            {
                if (targetTaskField.Piece != null)
                {
                    //target field has a piece on it already
                    //return field info, piece info and held piece info
                    var fieldToReturn = GetTaskFieldInfo((int)targetField.X, (int)targetField.Y, out var pieceToReturn);
                    var heldPieceToReturn = new DTO.Piece
                    {
                        Id = heldPiecePawn.Id,
                        PlayerId = heldPiecePawn.Player.Id
                    };
                    var piecesToReturn = pieceToReturn.ToList();
                    piecesToReturn.Add(heldPieceToReturn);
                    return new DTO.Data
                    {
                        PlayerId = playerPawn.Id,
                        TaskFields = new List<DTO.TaskField> { fieldToReturn },
                        Pieces = piecesToReturn.ToList()
                    };
                }
                else
                {
                    //place piece on task field
                    board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, targetTaskField));

                    //return new field data
                    var fieldToReturn = GetTaskFieldInfo((int)targetField.X, (int)targetField.Y, out var pieceToReturn);
                    return new DTO.Data
                    {
                        PlayerId = playerPawn.Id,
                        TaskFields = new List<DTO.TaskField> { fieldToReturn },
                        Pieces = pieceToReturn.ToList()
                    };
                }
            }
            var targetGoalField = targetField as IGoalField;

            if (heldPiecePawn.Type == PieceType.Sham)
            {
                board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, null));
                //get piece-less goal field to return
                var GoalToReturn = GetGoalFieldInfo((int)targetGoalField.X, (int)targetGoalField.Y, out var pieces); //pieces is null because there's no held piece anymore
                GoalToReturn.Type = targetGoalField.Type;
                if (targetGoalField.Type == GoalFieldType.Goal)
                {
                    // detach player from piece
                    GetPlayerFromGameMessage(placeRequest).Piece = null;
                    playerPawn.Piece = null;
                    //if goal, make a non-goal
                    board.SetField(board.Factory.CreateGoalField(targetGoalField.X, targetGoalField.Y, targetGoalField.Team, DateTime.Now, playerPawn, GoalFieldType.NonGoal));

                    //and decrease goals to go
                    if (targetGoalField.Team == TeamColour.Red)
                        redGoalsToScore--;
                    else
                        blueGoalsToScore--;
                }
                return new DTO.Data
                {
                    PlayerId = playerPawn.Id
                };
            }
            else
            {
                //remove the piece from the player
                board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, null));
                //get piece-less goal field to return
                var GoalToReturn = GetGoalFieldInfo((int)targetGoalField.X, (int)targetGoalField.Y, out var pieces); //pieces is null because there's no held piece anymore
                GoalToReturn.Type = targetGoalField.Type;
                if (targetGoalField.Type == GoalFieldType.Goal)
                {
                    // detach player from piece
                    GetPlayerFromGameMessage(placeRequest).Piece = null;
                    //if goal, make a non-goal
                    board.SetField(board.Factory.CreateGoalField(targetGoalField.X, targetGoalField.Y, targetGoalField.Team, DateTime.Now, playerPawn, GoalFieldType.NonGoal));
                    //and decrease goals to go
                    if (targetGoalField.Team == TeamColour.Red)
                        redGoalsToScore--;
                    else
                        blueGoalsToScore--;
                }
                return new DTO.Data
                {
                    GameFinished = (redGoalsToScore == 0 || blueGoalsToScore == 0),
                    PlayerId = playerPawn.Id,
                    GoalFields = new List<DTO.GoalField> { GoalToReturn }
                };
            }
        }

        private DTO.Data PerformSynchronizedTestPiece(DTO.TestPiece testPieceRequest)
        {
            var playerPawn = GetPlayerFromGameMessage(testPieceRequest);

            OnLog("test", DateTime.Now, gameId, playerPawn.Id, testPieceRequest.PlayerGuid, playerPawn.Team, playerPawn.Type);

            IPiece heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { PlayerId = playerPawn.Id }; //player wanted to test inaccessible piece
            }
            var result = new DTO.Data
            {
                PlayerId = playerPawn.Id,
                Pieces = new List<DTO.Piece> {
                        new DTO.Piece
                        {
                            Id = heldPiecePawn.Id,
                            Timestamp = DateTime.Now,
                            PlayerId = playerPawn.Id,
                            Type = heldPiecePawn.Type
                        }
                    }
            };
            return result;
        }

        #endregion

        #region IGameMaster

        public void DisconnectPlayer(DTO.PlayerDisconnected playerDisconnected)
        {
            var player = board.GetPlayer(playerDisconnected.PlayerId);
            board.SetPlayer(board.Factory.CreatePlayer(player.Id, player.Team, player.Type, DateTime.Now, null, player.Piece));
            var guidId = playerGuidToId.First(kvp => kvp.Value == playerDisconnected.PlayerId);
            playerGuidToId.Remove(guidId.Key);
            OnLog("Disconnected", DateTime.Now, gameId, guidId.Value, guidId.Key, player.Team, player.Type);
        }
		public DTO.RegisteredGames PerformConfirmGameRegistration() => new DTO.RegisteredGames()
		{
			GameInfo = new List<DTO.GameInfo>
				{
					new DTO.GameInfo{
						BlueTeamPlayers = config.GameDefinition.NumberOfPlayersPerTeam,
						RedTeamPlayers = config.GameDefinition.NumberOfPlayersPerTeam,
						GameName = config.GameDefinition.GameName
					}
				}
		};
		public DTO.PlayerMessage PerformJoinGame( DTO.JoinGame joinGame )
        {
            if (joinGame.GameName != config.GameDefinition.GameName
                || playerGuidToId.Keys.Count == config.GameDefinition.NumberOfPlayersPerTeam * 2)
            {
                var rejectingMessage = new DTO.RejectJoiningGame() { GameName = joinGame.GameName, PlayerId = joinGame.PlayerId };
                return rejectingMessage;
            }

            int totalNumberOfPlayersOfSameColour = board.Players.Where(player => player.Team == joinGame.PreferredTeam).Count();

            // if maximum player count reached then force change of teams
            if (config.GameDefinition.NumberOfPlayersPerTeam == totalNumberOfPlayersOfSameColour)
            {
                joinGame.PreferredTeam = joinGame.PreferredTeam == TeamColour.Blue ? TeamColour.Red : TeamColour.Blue;
            }

            bool teamAlreadyHasLeader = board.Players.Where(player => player.Team == joinGame.PreferredTeam && player.Type == Shared.Enums.PlayerRole.Leader).Count() > 0;

            // if there is a leader already then modify the request accordingly
            if ( teamAlreadyHasLeader && joinGame.PreferredRole == PlayerRole.Leader)
            {
                joinGame.PreferredRole = Shared.Enums.PlayerRole.Member;
            }


            ulong id = joinGame.PlayerId;//GenerateNewPlayerID();
            string guid = GenerateNewPlayerGUID();
            playerGuidToId.Add(guid, id);
            var fieldToPlacePlayer = GetAvailableFieldByTeam(joinGame.PreferredTeam);
            var generatedPlayer = board.Factory.CreatePlayer(id, joinGame.PreferredTeam, joinGame.PreferredRole, DateTime.Now, fieldToPlacePlayer, null);
            board.SetPlayer(generatedPlayer);
            return new DTO.ConfirmJoiningGame()
            {
                GameId = gameId,
                PlayerId = id,
                PrivateGuid = guid,
                PlayerDefinition = new Shared.DTOs.Communication.Player()
                {
                    Id = id,
                    Team = joinGame.PreferredTeam,
                    Role = joinGame.PreferredRole
                }
            };
        }


        public DTO.Data PerformDiscover(DTO.Discover discoverRequest)
        {
            if (CheckWin(discoverRequest.PlayerGuid, out var finalMessage)) return finalMessage;
            var result = PerformSynchronizedDiscover(discoverRequest);
            Thread.Sleep((int)config.ActionCosts.DiscoverDelay);
            return result;
        }

		public DTO.Data PerformKnowledgeExchange( DTO.KnowledgeExchangeRequest knowledgeExchangeRequest ) =>
			// TODO: DTO.Data czy raczej DTO.PlayerMessage?
			DelaySynchronizedAction(
			() => PerformSynchronizedKnowledgeExchange( knowledgeExchangeRequest ),
			config.ActionCosts.KnowledgeExchangeDelay
			);

		public DTO.Data PerformMove(DTO.Move moveRequest)
        {
            if (CheckWin(moveRequest.PlayerGuid, out var finalMessage))
                return finalMessage;
            else
                return DelaySynchronizedAction(
                    () => PerformSynchronizedMove(moveRequest),
                    config.ActionCosts.MoveDelay
                    );
        }

        public DTO.Data PerformPickUp(DTO.PickUpPiece pickUpRequest)
        {
            if (CheckWin(pickUpRequest.PlayerGuid, out var finalMessage))
                return finalMessage;
            else
                return DelaySynchronizedAction(
                    () => PerformSynchronizedPickUp(pickUpRequest),
                    config.ActionCosts.PickUpDelay
                    );
        }

        public DTO.Data PerformDestroy(DTO.DestroyPiece destroyRequest)
        {
            if (CheckWin(destroyRequest.PlayerGuid, out var finalMessage))
                return finalMessage;
            else
                return DelaySynchronizedAction(
                    () => PerformSynchronizedDestroy(destroyRequest),
                    config.ActionCosts.DestroyDelay
                    );
        }

        public DTO.Data PerformPlace(DTO.PlacePiece placeRequest)
        {
            if (CheckWin(placeRequest.PlayerGuid, out var finalMessage))
                return finalMessage;
            else
                return DelaySynchronizedAction(
                    () => PerformSynchronizedPlace(placeRequest),
                    config.ActionCosts.PlacingDelay
                    );
        }

        public DTO.Data PerformTestPiece(DTO.TestPiece testPieceRequest)
        {
            if (CheckWin(testPieceRequest.PlayerGuid, out var finalMessage))
                return finalMessage;
            else
                return DelaySynchronizedAction(
                    () => PerformSynchronizedTestPiece(testPieceRequest),
                    config.ActionCosts.TestDelay
                    );
        }

        public DTO.Data Perform(DTO.GameMessage gameMessage)
        {
            switch (gameMessage)
            {
                case DTO.Move move:
                    return PerformMove(move);
                case DTO.Discover discover:
                    return PerformDiscover(discover);
                case DTO.TestPiece test:
                    return PerformTestPiece(test);
                case DTO.PlacePiece place:
                    return PerformPlace(place);
                case DTO.PickUpPiece pick:
                    return PerformPickUp(pick);
                default:
                    return new DTO.Data() { PlayerId = GetPlayerIdFromGuid(gameMessage.PlayerGuid) };
            }
        }

        public bool IsPlayerBusy(DTO.GameMessage message)
        {
            if (!playerGuidToId.ContainsKey(message.PlayerGuid) || playerBusy.ContainsKey(GetPlayerIdFromGuid(message.PlayerGuid)))
                return true;
            return false;
        }

		public void FreePlayer( DTO.GameMessage message ) => playerBusy.Remove( GetPlayerIdFromGuid( message.PlayerGuid ) );

		public void BlockPlayer( DTO.GameMessage message ) => playerBusy.Add( GetPlayerIdFromGuid( message.PlayerGuid ), message );

		public virtual event EventHandler<LogArgs> Log = delegate { };
        #endregion

        #region IBoard to DTO converters
        private DTO.Field GetFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            if (y < config.GameDefinition.GoalAreaLength
                || y >= config.GameDefinition.GoalAreaLength + config.GameDefinition.TaskAreaLength)
            {
                return GetGoalFieldInfo(x, y, out pieces);
            }
            else
            {
                return GetTaskFieldInfo(x, y, out pieces);
            }
        }

        private DTO.TaskField GetTaskFieldInfo(int x, int y, out DTO.Piece[] pieces, bool computeDistanceInParallel = false)
        {
            var piecesToReturn = new List<DTO.Piece>();
            var currentField = board.GetField((uint)x, (uint)y) as ITaskField;
            var fieldToReturn = new DTO.TaskField
            {
                X = (uint)x,
                Y = (uint)y,
                Timestamp = DateTime.Now,
            };
            if (currentField?.Piece != null) //piece on the board
            {
                fieldToReturn.PieceId = currentField.Piece.Id;
                piecesToReturn.Add(new DTO.Piece
                {
                    Id = currentField.Piece.Id,
                    Type = PieceType.Unknown,
                    Timestamp = DateTime.Now
                });
            }
            if (currentField?.Player != null)
            {
                fieldToReturn.PlayerId = currentField.Player.Id;
                fieldToReturn.PlayerIdSpecified = true;
                if (board.GetPlayer(currentField.Player.Id).Piece != null) //check for held piece
                    piecesToReturn.Add(new DTO.Piece
                    {
                        Id = board.GetPlayer(currentField.Player.Id).Piece.Id,
                        Type = PieceType.Unknown,
                        Timestamp = DateTime.Now,
                        PlayerId = currentField.Player.Id,
                        PlayerIdSpecified = true
                    });
            }

            fieldToReturn.DistanceToPiece = (int)board.Pieces.
                Where(piece => piece is IFieldPiece).
                Select(piece => piece as IFieldPiece).
                Where(fieldPiece => fieldPiece.Field != null).
                Min(fieldPiece => Math.Abs(fieldPiece.Field.X - x) + Math.Abs(fieldPiece.Field.Y - y));


            #region returning
            // pieces has an "out" parameter modifier
            pieces = piecesToReturn.ToArray();
            return fieldToReturn;
            #endregion
        }

        private DTO.GoalField GetGoalFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            pieces = new DTO.Piece[] { };
            var relevantField = board.GetField((uint)x, (uint)y) as IGoalField;
            var goalFieldToReturn = new DTO.GoalField
            {
                Timestamp = DateTime.Now,
                Type = GoalFieldType.Unknown,
                Team = relevantField.Team,
                X = (uint)x,
                Y = (uint)y
            };
            if (relevantField.Player != null)
            {
                goalFieldToReturn.PlayerId = relevantField.Player.Id;
                goalFieldToReturn.PlayerIdSpecified = true;
                if (relevantField.Player.Piece != null)
                {
                    var heldPiece = relevantField.Player.Piece;
                    pieces = new DTO.Piece[]{
                        new DTO.Piece()
                        {
                            Id = heldPiece.Id,
                            PlayerId = heldPiece.Player.Id,
                            PlayerIdSpecified = true,
                            Timestamp = DateTime.Now,
                            Type = PieceType.Unknown
                        }
                    };
                }
            }
			if( pieces is null )
				pieces = new DTO.Piece[] { };
            return goalFieldToReturn;
        }
        #endregion

        #region HelperMethods

        private T DelaySynchronizedAction<T>(Func<T> function, long milisecondDelay, int maximumExpectedExecutionTime)
            => DelaySynchronizedAction(function, milisecondDelay, (double)(milisecondDelay - maximumExpectedExecutionTime) / milisecondDelay);
        private T DelaySynchronizedAction<T>(Func<T> function, long milisecondDelay, double fraction = 0.85)
        {
            var then = DateTime.Now;
            T result;
            Task.Delay((int)(fraction * milisecondDelay));
            // synchronization part
            lock (board)
            {
                result = function();
            }
			int milisecondsToSleep = (int)(Math.Floor(milisecondDelay - (DateTime.Now - then).TotalMilliseconds));
            if (milisecondsToSleep > 0)
                Task.Delay(milisecondsToSleep);
            return result;
        }
        private ulong GenerateNewPlayerID() => (ulong)++playerIDcounter;

        private string GenerateNewPlayerGUID()
        {
            string guid;
            var random = new Random();
            do
            {
                guid = Guid.NewGuid().ToString();
            } while (playerGuidToId.Keys.Contains(guid));
            return guid;
        }

        private IField GetAvailableFieldByTeam(TeamColour preferredTeam)
        {
            var position = new DTO.Location();
            switch (preferredTeam)
            {
                case TeamColour.Red:
                    do
                    {
                        position = GenerateRandomPlaces(1, 0, board.Width, board.Height - config.GameDefinition.TaskAreaLength, board.Height).First();
                    } while (board.GetField(position).Player != null);
                    return board.GetField(position);
                case TeamColour.Blue:
                    do
                    {
                        position = GenerateRandomPlaces(1, 0, board.Width, 0, config.GameDefinition.TaskAreaLength).First();
                    } while (board.GetField(position).Player != null);
                    return board.GetField(position);
            }
            throw new ArgumentException("Invalid team colour");
        }

        private IList<IField> GetAvailableFieldsByTeam(TeamColour preferredTeam, int n = 1)
        {
            var list = new List<IField>(n);
            for (int i = 0; i < n; i++)
            {
                list.Add(GetAvailableFieldByTeam(preferredTeam));
            }
            return list;
        }

        private ulong GetPlayerIdFromGuid(string guid) => playerGuidToId.FirstOrDefault(pair => pair.Key == guid).Value;

        private TeamColour GetTeamColorFromCoordinateY(int y) => y < board.GoalsHeight ? TeamColour.Blue : TeamColour.Red;

        private IPlayer GetPlayerFromGameMessage(DTO.GameMessage message)
        {
            if (!playerGuidToId.ContainsKey(message.PlayerGuid))
                return null;
            return board.GetPlayer(GetPlayerIdFromGuid(message.PlayerGuid));
        }

        private PieceType GetRandomPieceType() =>
            new Random().NextDouble() < config.GameDefinition.ShamProbability ?
            PieceType.Sham : PieceType.Normal;

        /// <summary>
        /// Returns mathematically correct uniformly generated coordinates
        /// It runs in linear amortised time and uses space proportional to the number of elements
        /// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        /// </summary>
        private List<DTO.Location> GenerateRandomPlaces(
            uint n, uint minXInclusive, uint maxXExclusive, uint minYInclusive, uint maxYExclusive)
        {


            if (maxXExclusive <= minXInclusive || maxYExclusive <= minYInclusive)
            {
                throw new ArgumentOutOfRangeException("Incorrectly defined rectangle");
            }

            int totalFieldCount = (int)((maxXExclusive - minXInclusive) * (maxYExclusive - minYInclusive));
            var placeToPieceId = new Dictionary<int, int>();

            for (int i = 0; i < n; i++)
            {
                placeToPieceId.Add(i, i);
            }

            for (int i = 0; i < totalFieldCount - 1; i++)
            {
				int randomTargetPlace = random.Next(i, totalFieldCount);

                if (placeToPieceId.Keys.Contains(i))
                {
                    if (placeToPieceId.Keys.Contains(randomTargetPlace))
                    {

						int tmpId = placeToPieceId[randomTargetPlace];
                        placeToPieceId[randomTargetPlace] = placeToPieceId[i];
                        placeToPieceId[i] = tmpId;
                    }
                    else
                    {
                        placeToPieceId[randomTargetPlace] = placeToPieceId[i];
                        placeToPieceId.Remove(i);
                    }
                }
                else
                {
                    if (placeToPieceId.Keys.Contains(randomTargetPlace))
                    {
                        placeToPieceId[i] = placeToPieceId[randomTargetPlace];
                        placeToPieceId.Remove(randomTargetPlace);
                    }
                }
            }

            var coordinateListToReturn = new DTO.Location[n];
            foreach (var keyValue in placeToPieceId)
            {
                coordinateListToReturn[keyValue.Value] = new DTO.Location()
                {
                    X = (uint)(minXInclusive + (keyValue.Key % (maxXExclusive - minXInclusive))),
                    Y = (uint)(minYInclusive + (keyValue.Key / (maxXExclusive - minXInclusive)))
                };

            }

            return coordinateListToReturn.ToList();
        }

        private bool CheckWin(string guid, out DTO.Data message)
        {
            if (redGoalsToScore == 0 || blueGoalsToScore == 0)
            {
                var player = board.GetPlayer(GetPlayerIdFromGuid(guid));
                bool win = (redGoalsToScore == 0 && player.Team == TeamColour.Red) || (blueGoalsToScore == 0 && player.Team == TeamColour.Blue);
                OnLog(win ? "Victory" : "Defeat", DateTime.Now, gameId, player.Id, guid, player.Team, player.Type);
                message = new DTO.Data
                {
                    GameFinished = true,
                    PlayerId = GetPlayerIdFromGuid(guid)
                };
                return true;
            }
            else
            {
                message = null;
                return false;
            }
        }

        protected void OnLog(string type, DateTime timestamp, ulong gameId, ulong playerId, string playerGuid, TeamColour colour, Shared.Enums.PlayerRole role)
            => EventHelper.OnEvent(this, Log, new LogArgs(type, timestamp, gameId, playerId, playerGuid, colour, role));
        #endregion

        #region TEMPORARY METHOD to be changed in future stages
        public DTO.Game GetGame(string guid)
        {
            var player = board.GetPlayer(GetPlayerIdFromGuid(guid));
            var players = board.Players.Select(p => new DTO.Player()
            {
                Id = p.Id,
                Team = p.Team,
                Role = p.Type
            });

            return new DTO.Game()
            {
                PlayerId = player.Id,
                Players = players.ToList(),
                Board = new DTO.GameBoard() { GoalsHeight = board.GoalsHeight, TasksHeight = board.TasksHeight, Width = board.Width },
                PlayerLocation = new DTO.Location() { X = player.GetX().Value, Y = player.GetY().Value }
            };
        }
        #endregion

    }
}