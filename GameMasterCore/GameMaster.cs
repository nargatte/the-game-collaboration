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
using Shared.Components.Extensions;
using Config = Shared.Messages.Configuration;
using DTO = Shared.Messages.Communication;
using System.Threading;

namespace GameMasterCore
{
    public class BlockingGameMaster : IGameMaster
    {
        IBoard board;
        Dictionary<string, ulong> playerGuidToId;
        int playerIDcounter = 0;
        int pieceIDcounter = 0;
        Config.GameMasterSettings config;
        public Dictionary<ulong, DTO.Game> game { get; set; } // for process game by communication substitute 

        public BlockingGameMaster()
        {
            playerGuidToId = new Dictionary<string, ulong>();

            //prepare default config
            config = GenerateDefaultConfig();

            //generate board itself from config
            board = PrepareBoard(new BoardComponentFactory());
        }

        public BlockingGameMaster(Config.GameMasterSettings _config, IBoardComponentFactory _boardComponentFactory)
        {
            playerGuidToId = new Dictionary<string, ulong>();

            config = _config;
            board = PrepareBoard(_boardComponentFactory);
        }

        #region Preparation
        private Config.GameMasterSettings GenerateDefaultConfig()
        {
            var result = new Config.GameMasterSettings
            {
                ActionCosts = new Config.GameMasterSettingsActionCosts(), //default ActionCosts
                GameDefinition = new Config.GameMasterSettingsGameDefinition(), //default GameDefinition, without Goals(!) and Name
            };

            //generate Goals for default config without goals
            var goalLocationsBlue = GenerateRandomPlaces(6, 0,
                result.GameDefinition.BoardWidth, 0,
                result.GameDefinition.GoalAreaLength
                );
            var goalLocationsRed = GenerateRandomPlaces(6, 0, result.GameDefinition.BoardWidth,
                result.GameDefinition.GoalAreaLength + result.GameDefinition.TaskAreaLength,
                result.GameDefinition.TaskAreaLength + 2 * result.GameDefinition.GoalAreaLength
                );

            result.GameDefinition.Goals = goalLocationsBlue.Select(location =>
                new Config.GoalField
                {
                    team = TeamColour.Blue,
                    type = GoalFieldType.Goal,
                    x = location.x,
                    y = location.y
                }
            ).Concat(goalLocationsRed.Select(location =>
                new Config.GoalField
                {
                    team = TeamColour.Red,
                    type = GoalFieldType.Goal,
                    x = location.x,
                    y = location.y
                }
            )).ToArray();

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
                result.SetField(
                    result.Factory.CreateGoalField(gf.x, gf.y, gf.team, DateTime.Now, null, GoalFieldType.Goal)
                    );
            //set the rest of GoalArea fields as NonGoals
            foreach (var f in result.Fields)
                if (f is IGoalField gf && gf.Type == GoalFieldType.Unknown)
                    result.SetField(
                        result.Factory.CreateGoalField(gf.X, gf.Y, gf.Team, DateTime.Now, null, GoalFieldType.NonGoal)
                        );
                        
            GenerateRandomPlaces(
                config.GameDefinition.InitialNumberOfPieces,
                0, board.Width, board.TasksHeight, board.Height - board.TasksHeight).ForEach(
                    place => board.SetPiece(board.Factory.CreateFieldPiece((ulong)++pieceIDcounter, GetRandomPieceType(), DateTime.Now, (ITaskField)board.GetField(place)))
                );

            return result;
        }
        #endregion

        #region Synced helper methods for IGameMaster
        private DTO.Data PerformSynchronizedDiscover(DTO.Discover discoverRequest)
        {
            lock (board)
            {
                IPlayer playerPawn = GetPlayerFromGameMessage(discoverRequest);
                //Prepare partial result structures
                List<DTO.TaskField> resultFields = new List<DTO.TaskField>();
                List<DTO.Piece> resultPieces = new List<DTO.Piece>();
                //Perform discover on 3x3 
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
        }

        private DTO.Data PerformSynchronizedKnowledgeExchange(DTO.KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            //TODO: knowledge exchange
            throw new NotImplementedException();
        }

        private DTO.Data PerformSynchronizedMove(DTO.Move moveRequest)
        {
            lock (board)
            {
                IPlayer playerPawn = GetPlayerFromGameMessage(moveRequest);

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
                //check for invalid moves
                if (targetField == null
                    || (targetField is IGoalField gf && gf.Team != playerPawn.Team))
                {
                    //trying to move outside the board or to other team's goal area - do not move the player
                    return new DTO.Data
                    {
                        playerId = playerPawn.Id,
                        PlayerLocation = new DTO.Location { x = playerPawn.GetX().Value, y = playerPawn.GetY().Value }
                    };
                }
                if (targetField.Player != null)
                {
                    //field is occupied - do not move the player, return info about the occupied field
                    var occupiedField = GetFieldInfo(targetX, targetY, out DTO.Piece[] pieces);
                    return new DTO.Data
                    {
                        playerId = playerPawn.Id,
                        PlayerLocation = new DTO.Location { x = playerPawn.GetX().Value, y = playerPawn.GetY().Value },
                        Pieces = pieces,
                        GoalFields = (occupiedField is DTO.GoalField) ? new DTO.GoalField[] { occupiedField as DTO.GoalField } : null,
                        TaskFields = (occupiedField is DTO.TaskField) ? new DTO.TaskField[] { occupiedField as DTO.TaskField } : null
                    };
                }

                //move
                board.SetPlayer(board.Factory.CreatePlayer(playerPawn.Id, playerPawn.Team, playerPawn.Type, DateTime.Now, targetField, playerPawn.Piece));

                //return information about current field and new player location
                var currentField = GetFieldInfo(targetX, targetY, out DTO.Piece[] currentPieces);
                DTO.Data result = new DTO.Data
                {
                    playerId = playerPawn.Id,
                    PlayerLocation = new DTO.Location { x = (uint)targetX, y = (uint)targetY },
                    Pieces = currentPieces,
                    GoalFields = (currentField is DTO.GoalField) ? new DTO.GoalField[] { currentField as DTO.GoalField } : null,
                    TaskFields = (currentField is DTO.TaskField) ? new DTO.TaskField[] { currentField as DTO.TaskField } : null
                };
                return result;
            }
        }

        private DTO.Data PerformSynchronizedPickUp(DTO.PickUpPiece pickUpRequest)
        {
            lock (board)
            {
                IPlayer playerPawn = GetPlayerFromGameMessage(pickUpRequest);
                ITaskField field = (board.GetField(playerPawn.GetX().Value, playerPawn.GetY().Value) as TaskField);
                IPiece piece = field.Piece;

                if (piece == null)
                {
                    return new DTO.Data
                    {
                        playerId = playerPawn.Id
                    };
                }

                board.SetPiece(board.Factory.CreatePlayerPiece(piece.Id, piece.Type, DateTime.Now, playerPawn));

                //prepare result data
                DTO.Data result = new DTO.Data
                {
                    playerId = playerPawn.Id,
                    Pieces = new DTO.Piece[]
                    {
                    new DTO.Piece
                    {
                        id = piece.Id,
                        playerId = playerPawn.Id,
                        timestamp = DateTime.Now,
                        type = PieceType.Unknown
                    }
                    }
                };
                return result;
            }
        }

        private DTO.Data PerformSynchronizedPlace(DTO.PlacePiece placeRequest)
        {
            lock (board)
            {
                IPlayer playerPawn = GetPlayerFromGameMessage(placeRequest);
                IPlayerPiece heldPiecePawn = playerPawn.Piece;
                if (heldPiecePawn == null)
                {
                    return new DTO.Data { playerId = playerPawn.Id }; //player wanted to place inaccessible piece
                }
                IField targetField = playerPawn.Field;
                if (targetField is ITaskField targetTaskField)
                {
                    if (targetTaskField.Piece != null)
                    {
                        //target field has a piece on it already
                        //return field info, piece info and held piece info
                        DTO.TaskField fieldToReturn = GetTaskFieldInfo((int)targetField.X, (int)targetField.Y, out DTO.Piece[] pieceToReturn);
                        DTO.Piece heldPieceToReturn = new DTO.Piece
                        {
                            id = heldPiecePawn.Id,
                            playerId = heldPiecePawn.Player.Id
                        };
                        var piecesToReturn = pieceToReturn.ToList();
                        piecesToReturn.Add(heldPieceToReturn);
                        return new DTO.Data
                        {
                            playerId = playerPawn.Id,
                            TaskFields = new DTO.TaskField[] { fieldToReturn },
                            Pieces = piecesToReturn.ToArray()
                        };
                    }
                    else
                    {
                        //place piece on task field
                        board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, targetTaskField));

                        //return new field data
                        DTO.TaskField fieldToReturn = GetTaskFieldInfo((int)targetField.X, (int)targetField.Y, out DTO.Piece[] pieceToReturn);
                        return new DTO.Data
                        {
                            playerId = playerPawn.Id,
                            TaskFields = new DTO.TaskField[] { fieldToReturn },
                            Pieces = pieceToReturn
                        };
                    }
                }
                var targetGoalField = targetField as IGoalField;
                
                if(heldPiecePawn.Type == PieceType.Sham)
                {
                    board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, null));
                    return new DTO.Data
                    {
                        playerId = playerPawn.Id
                    };
                }

                var GoalToReturn = GetGoalFieldInfo((int)targetGoalField.X, (int)targetGoalField.Y);
                if (targetGoalField.Type == GoalFieldType.Goal)
                {
                    //if goal, make a non-goal and remove piece from the player
                    board.SetPiece(board.Factory.CreateFieldPiece(heldPiecePawn.Id, heldPiecePawn.Type, DateTime.Now, null));
                    board.SetField(board.Factory.CreateGoalField(targetGoalField.X, targetGoalField.Y, targetGoalField.Team, DateTime.Now, playerPawn, GoalFieldType.NonGoal));
                }
                return new DTO.Data
                {
                    playerId = playerPawn.Id,
                    GoalFields = new DTO.GoalField[] { GoalToReturn }
                };
            }
        }

        private DTO.Data PerformSynchronizedTestPiece(DTO.TestPiece testPieceRequest)
        {
            lock (board)
            {
                IPlayer playerPawn = GetPlayerFromGameMessage(testPieceRequest);
                IPiece heldPiecePawn = playerPawn.Piece;
                if (heldPiecePawn == null)
                {
                    return new DTO.Data { playerId = playerPawn.Id }; //player wanted to test inaccessible piece
                }
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
        }

        #endregion

        #region IGameMaster
        public DTO.RegisteredGames PerformConfirmGameRegistration()
        {
            return new DTO.RegisteredGames()
            {
                GameInfo = new DTO.GameInfo[]
                {
                    new DTO.GameInfo{
                        blueTeamPlayers = (ulong)board.Players.Select(player => player.Team == TeamColour.Blue).Count(),
                        redTeamPlayers = (ulong)board.Players.Select(player => player.Team == TeamColour.Red).Count(),
                        gameName = config.GameDefinition.GameName
                    }
                }
            };
        }
        public DTO.PlayerMessage PerformJoinGame(DTO.JoinGame joinGame)
        {
            if (joinGame.gameName != config.GameDefinition.GameName
                || playerGuidToId.Keys.Count == config.GameDefinition.NumberOfPlayersPerTeam * 2)
            {
                // the player shouldn't know his id if he's been rejected :/
                var rejectingMessage = new DTO.RejectJoiningGame() { gameName = joinGame.gameName, playerId = 0 };
                return rejectingMessage;
            }

            int totalNumberOfPlayersOfSameColour = board.Players.Where(player => player.Team == joinGame.preferredTeam).Count();

            // if maximum player count reached then force change of teams
            if (config.GameDefinition.NumberOfPlayersPerTeam == totalNumberOfPlayersOfSameColour)
            {
                joinGame.preferredTeam = joinGame.preferredTeam == TeamColour.Blue ? TeamColour.Red : TeamColour.Blue;
            }

            bool teamAlreadyHasLeader = board.Players.Where(player => player.Team == joinGame.preferredTeam && player.Type == PlayerType.Leader).Count() > 0;

            // if there is a leader already then modify the request accordingly
            if (teamAlreadyHasLeader && joinGame.preferredRole == PlayerType.Leader)
            {
                joinGame.preferredRole = PlayerType.Member;
            }



            ulong id = GenerateNewPlayerID();
            var fieldToPlacePlayer = GetAvailableFieldByTeam(joinGame.preferredTeam);
            var generatedPlayer = board.Factory.CreatePlayer(id, joinGame.preferredTeam, joinGame.preferredRole, DateTime.Now, fieldToPlacePlayer, null);
            board.SetPlayer(generatedPlayer);
            return new DTO.ConfirmJoiningGame()
            {
                gameId = 1,
                playerId = id,
                privateGuid = GenerateNewPlayerGUID(),
                PlayerDefinition = new DTO.Player()
                {
                    id = id,
                    team = joinGame.preferredTeam,
                    type = joinGame.preferredRole
                }
            };
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

        public DTO.Data PerformDiscover(DTO.Discover discoverRequest)
        {
            DTO.Data result = PerformSynchronizedDiscover(discoverRequest);
            Thread.Sleep((int)config.ActionCosts.DiscoverDelay);
            return result;
        }

        public DTO.Data PerformKnowledgeExchange(DTO.KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            // TODO: DTO.Data czy raczej DTO.PlayerMessage?
            DTO.Data result = PerformSynchronizedKnowledgeExchange(knowledgeExchangeRequest);
            Thread.Sleep((int)config.ActionCosts.KnowledgeExchangeDelay);
            return result;
        }

        public DTO.Data PerformMove(DTO.Move moveRequest)
        {
            DTO.Data result = PerformSynchronizedMove(moveRequest);
            Thread.Sleep((int)config.ActionCosts.MoveDelay);
            return result;
        }

        public DTO.Data PerformPickUp(DTO.PickUpPiece pickUpRequest)
        {
            DTO.Data result = PerformSynchronizedPickUp(pickUpRequest);
            Thread.Sleep((int)config.ActionCosts.PickUpDelay);
            return result;
        }

        public DTO.Data PerformPlace(DTO.PlacePiece placeRequest)
        {
            DTO.Data result = PerformSynchronizedPlace(placeRequest);
            Thread.Sleep((int)config.ActionCosts.PlacingDelay);
            return result;
        }

        public DTO.Data PerformTestPiece(DTO.TestPiece testPieceRequest)
        {
            DTO.Data result = PerformSynchronizedTestPiece(testPieceRequest);
            Thread.Sleep((int)config.ActionCosts.TestDelay);
            return result;
        }
        #endregion

        #region IBoard to DTO converters
        private DTO.Field GetFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            if (y < config.GameDefinition.GoalAreaLength
                || y > config.GameDefinition.GoalAreaLength + config.GameDefinition.TaskAreaLength)
            {
                pieces = null;
                return GetGoalFieldInfo(x, y);
            }
            return GetTaskFieldInfo(x, y, out pieces);
        }

        private DTO.TaskField GetTaskFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            var piecesToReturn = new List<DTO.Piece>();
            var currentField = board.GetField((uint)x, (uint)y) as ITaskField;
            var fieldToReturn = new DTO.TaskField
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

            // może ewentualnie dodać AsParallel().
            fieldToReturn.distanceToPiece = (int)board.Pieces.
                Where(piece => piece is IFieldPiece).
                Select(piece => piece as IFieldPiece).
                Min(fieldPiece => Math.Abs(fieldPiece.Field.X - x) + Math.Abs(fieldPiece.Field.Y - y));

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
                team = GetTeamColorFromCoordinateY(y),
                x = (uint)x,
                y = (uint)y
            };

            return goalFieldToReturn;
        }
        #endregion

        #region HelperMethods
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

        private ulong GetPlayerIdFromGuid(string guid) => playerGuidToId.FirstOrDefault(pair => pair.Key == guid).Value;

        private TeamColour GetTeamColorFromCoordinateY(int y) => y < board.GoalsHeight ? TeamColour.Blue : TeamColour.Red;

        private IPlayer GetPlayerFromGameMessage(DTO.GameMessage message)
        {
            // TODO: verify gameID
            return board.GetPlayer(GetPlayerIdFromGuid(message.playerGuid));
        }

        private PieceType GetRandomPieceType() =>
            new Random().NextDouble() < config.GameDefinition.ShamProbability ?
            PieceType.Sham : PieceType.Normal;

        /// <summary>
        /// Returns mathematically correct uniformly generated coordinates
        /// </summary>
        private List<DTO.Location> GenerateRandomPlaces(
            uint n, uint minXInclusive, uint maxXExclusive, uint minYInclusive, uint maxYExclusive)
        {
            if (maxXExclusive <= minYInclusive || maxYExclusive <= minYInclusive)
            {
                throw new ArgumentOutOfRangeException("Incorrectly defined rectangle");
            }

            int totalFieldCount = (int)((maxXExclusive - minXInclusive) * (maxYExclusive - minYInclusive));
            var random = new Random();
            var placeToPieceId = new Dictionary<int, int>();

            for (int i = 0; i < n; i++)
            {
                placeToPieceId.Add(i, i);
            }

            for (int i = 0; i < n; i++)
            {
                var randomPlace = random.Next(0, totalFieldCount);
                if (placeToPieceId.Keys.Contains(randomPlace))
                {

                    var tmpId = placeToPieceId[randomPlace];
                    placeToPieceId[randomPlace] = placeToPieceId[i];
                    placeToPieceId[i] = tmpId;
                }
                else
                {
                    placeToPieceId[randomPlace] = i;
                    placeToPieceId.Remove(i);
                }
            }
            var coordinateListToReturn = new List<DTO.Location>((int)n);
            foreach (var keyValue in placeToPieceId)
            {
                coordinateListToReturn[keyValue.Value] = new DTO.Location()
                {
                    x = (uint)(minXInclusive + (keyValue.Key / (maxYExclusive - minYInclusive))),
                    y = (uint)(minYInclusive + (keyValue.Key / (maxXExclusive - minXInclusive)))
                };

            }

            return coordinateListToReturn;
        }
        #endregion
    }
}
