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
            //prepare config
            config = PrepareDefaultConfig();

            //generate board itself from config
            board = PrepareBoard();
        }

        #region Preparation
        private Config.GameMasterSettings PrepareDefaultConfig()
        {
            var result = new Config.GameMasterSettings
            {
                ActionCosts = new Config.GameMasterSettingsActionCosts(), //default ActionCosts
                GameDefinition = new Config.GameMasterSettingsGameDefinition(), //default GameDefinition, without Goals(!) and Name
            };
            //generate Goals for default config without goals
            var goalLocationsBlue = GenerateRandomPlaces(6, 0, result.GameDefinition.BoardWidth, 0, result.GameDefinition.GoalAreaLength);
            var goalLocationsRed = GenerateRandomPlaces(6, 0, result.GameDefinition.BoardWidth, result.GameDefinition.GoalAreaLength + result.GameDefinition.TaskAreaLength, result.GameDefinition.TaskAreaLength + 2 * result.GameDefinition.GoalAreaLength);
            List<Config.GoalField> goals = new List<Config.GoalField>(goalLocationsBlue.Select(loc => new Config.GoalField { team = TeamColour.Blue, type = GoalFieldType.Goal, x = loc.x, y = loc.y }));
            goals.AddRange(goalLocationsRed.Select(loc => new Config.GoalField { team = TeamColour.Red, type = GoalFieldType.Goal, x = loc.x, y = loc.y }));
            result.GameDefinition.Goals = goals.ToArray();
            return result;
        }

        private IBoard PrepareBoard()
        {
            IBoard result = new Board(
                config.GameDefinition.BoardWidth,
                config.GameDefinition.TaskAreaLength,
                config.GameDefinition.GoalAreaLength);
            //set Goals from configuration
            foreach (var gf in config.GameDefinition.Goals)
                result.SetField(
                    new GoalField(gf.x, gf.y, gf.team, DateTime.Now, type: GoalFieldType.Goal)
                    );
            //set the rest of GoalArea fields as NonGoals
            foreach (var f in result.Fields)
                if (f is GoalField gf && gf.Type == GoalFieldType.Unknown)
                    result.SetField(
                        new GoalField(gf.X, gf.Y, gf.Team, DateTime.Now, type: GoalFieldType.NonGoal)
                        );

            //TODO: place players on the board
            var randomBluePlaces = GenerateRandomPlaces(
                config.GameDefinition.NumberOfPlayersPerTeam,
                0, board.Width, 0, board.TasksHeight);

            var randomRedPlaces = GenerateRandomPlaces(
                config.GameDefinition.NumberOfPlayersPerTeam,
                0, board.Width, board.Height - board.TasksHeight, board.Height);

            var players = new List<IPlayer>();
            var randomBluePlaceIterator = randomBluePlaces.GetEnumerator();
            var randomRedPlaceIterator = randomBluePlaces.GetEnumerator();
            // place blue players
            for (int i = 0; i < config.GameDefinition.NumberOfPlayersPerTeam; i++)
            {
                if (!randomBluePlaceIterator.MoveNext())
                {
                    // shouldn't happen
                    break;
                }

                // create blue players and add them to the board with randomBluePlaceIterator.Current position

            }
            // do the same with red players

            //TODO: generate and place pieces

            //GenerateRandomPlaces(
            //    config.GameDefinition.InitialNumberOfPieces,
            //    0, board.Width, board.TasksHeight, board.Height - board.TasksHeight).ForEach(
            //        place => board.SetPiece(new FieldPiece(???))
            //    );

            return result;
        }
        #endregion

        #region IGameMaster
        public DTO.Data PerformDiscover(DTO.Discover discoverRequest)
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

        public DTO.Data PerformConfirmGameRegistration(DTO.RegisteredGames registeredGames) => throw new NotImplementedException();
        public DTO.Data PerformJoinGame(DTO.JoinGame joinGame)
        {
            var result = new DTO.Data();

            if (joinGame.gameName != config.GameDefinition.GameName)
            {
                // TODO: rather a "rejecting" message
                return result;
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


            ulong id = GenerateNewID();
            var generatedPlayer = new Player(id, joinGame.preferredTeam, joinGame.preferredRole);
            // TODO: check for return type bool?
            board.SetPlayer(generatedPlayer);
            result.playerId = id;
            
            return result;
        }

        private ulong GenerateNewID()
        {
            ulong id;
            var random = new Random();
            do
            {
                id = (ulong)((long)(random.Next() * int.MaxValue) + random.Next());
            } while (playerGuidToId.Values.Contains(id));
            return id;
        }

        public DTO.Data PerformKnowledgeExchange(DTO.KnowledgeExchangeRequest knowledgeExchangeRequest)
        {
            //TODO: knowledge exchange
            throw new NotImplementedException();
        }

        public DTO.Data PerformMove(DTO.Move moveRequest)
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
            board.SetPlayer(new Player(playerPawn.Id, playerPawn.Team, playerPawn.Type, DateTime.Now, targetField, playerPawn.Piece));

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


        public DTO.Data PerformPickUp(DTO.PickUpPiece pickUpRequest)
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

            //TODO: perform pickup itself [can't do now because of not clear info about SetPlayer and SetField usage]
            //set player to contain the obtained piece
            //??? board.SetPlayer(new Player(playerPawn.Id, playerPawn.Team, playerPawn.Type, field:playerPawn.Field, piece: new PlayerPiece())
            //set field to not contain the piece anymore


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

        public DTO.Data PerformPlace(DTO.PlacePiece placeRequest)
        {
            IPlayer playerPawn = GetPlayerFromGameMessage(placeRequest);
            IPiece heldPiecePawn = playerPawn.Piece;
            if (heldPiecePawn == null)
            {
                return new DTO.Data { playerId = playerPawn.Id }; //player wanted to place inaccessible piece
            }
            IField targetField = playerPawn.Field;
            DTO.Field fieldToReturn = GetFieldInfo((int)targetField.X, (int)targetField.Y, out DTO.Piece[] pieces);
            if (targetField is ITaskField targetTaskField)
            {
                if (targetTaskField.Piece != null)
                {
                    //target field has a piece on it already
                    //TODO: return field info, piece info and held piece info
                }
                //TODO: place the Piece [more info on board's methods needed]
            }
            var targetGoalField = targetField as IGoalField;
            //zobacz czy może odłożyć
            //zobacz czy zyskuje punkt
            //zobacz czy koniec gry
            throw new NotImplementedException();
        }

        public DTO.Data PerformTestPiece(DTO.TestPiece testPieceRequest)
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
        #endregion

        #region IBoard to DTO converters
        private DTO.Field GetFieldInfo(int x, int y, out DTO.Piece[] pieces)
        {
            if (y < config.GameDefinition.GoalAreaLength || y > config.GameDefinition.GoalAreaLength + config.GameDefinition.TaskAreaLength)
            {
                pieces = null;
                return GetGoalFieldInfo(x, y);
            }
            return GetTaskFieldInfo(x, y, out pieces);
        }

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
        private ulong GetPlayerIdFromGuid(string guid) => playerGuidToId.FirstOrDefault(pair => pair.Key == guid).Value;

        private TeamColour GetTeamColorFromCoordinateY(int y) => y < board.GoalsHeight ? TeamColour.Blue : TeamColour.Red;

        private IPlayer GetPlayerFromGameMessage(DTO.GameMessage message) => board.GetPlayer(GetPlayerIdFromGuid(message.playerGuid));

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
