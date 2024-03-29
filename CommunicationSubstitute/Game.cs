﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using GameMasterCore;
//using PlayerCore;
//using Shared.Components.Factories;
//using Shared.Enums;
//using Shared.Interfaces;
//using Shared.Messages.Communication;
//using Shared.DTO.Configuration;

//namespace CommunicationSubstitute
//{
//    public class Game
//    {
//        private const bool ShowData = true;
//        private const bool BigCosts = true;


//        public IGameMaster GameMaster;

//        public GameInfo GameInfo;

//        public PlayerInGame[] BluePlayers;
//        public PlayerInGame[] RedPlayers;
//        public Shared.DTO.Communication.ConfirmJoiningGame[] BlueConfirms;
//        public Shared.DTO.Communication.ConfirmJoiningGame[] RedConfirms;
//        public Thread[] BlueThreads;
//        public Thread[] RedThreads;

//        readonly Dictionary<ulong, bool> EndGame = new Dictionary<ulong, bool>();

//        #region From GameMaster

//        Random random = new Random(123456);

//        private List<Shared.Messages.Communication.Location> GenerateRandomPlaces(
//            uint n, uint minXInclusive, uint maxXExclusive, uint minYInclusive, uint maxYExclusive)
//        {
//            if (maxXExclusive <= minXInclusive || maxYExclusive <= minYInclusive)
//            {
//                throw new ArgumentOutOfRangeException("Incorrectly defined rectangle");
//            }

//            int totalFieldCount = (int)((maxXExclusive - minXInclusive) * (maxYExclusive - minYInclusive));
//            var placeToPieceId = new Dictionary<int, int>();

//            for (int i = 0; i < n; i++)
//            {
//                placeToPieceId.Add(i, i);
//            }

//            for (int i = 0; i < n; i++)
//            {
//                var randomPlace = random.Next(0, totalFieldCount);
//                if (placeToPieceId.Keys.Contains(randomPlace))
//                {

//                    var tmpId = placeToPieceId[randomPlace];
//                    placeToPieceId[randomPlace] = placeToPieceId[i];
//                    placeToPieceId[i] = tmpId;
//                }
//                else
//                {
//                    placeToPieceId[randomPlace] = placeToPieceId[i];
//                    placeToPieceId.Remove(i);
//                }
//            }
//            var coordinateListToReturn = new Shared.Messages.Communication.Location[n];
//            foreach (var keyValue in placeToPieceId)
//            {
//                coordinateListToReturn[keyValue.Value] = new Shared.Messages.Communication.Location()
//                {
//                    x = (uint)(minXInclusive + (keyValue.Key % (maxXExclusive - minXInclusive))),
//                    y = (uint)(minYInclusive + (keyValue.Key / (maxXExclusive - minXInclusive)))
//                };

//            }

//            return coordinateListToReturn.ToList();
//        }

//        private Shared.DTO.Configuration.GameMasterSettings GenerateDefaultConfig()
//        {
//            var result = new Shared.DTO.Configuration.GameMasterSettings
//            {
//                ActionCosts = new Shared.DTO.Configuration.GameMasterSettingsActionCosts(), // default ActionCosts
//                GameDefinition = new Shared.DTO.Configuration.GameMasterSettingsGameDefinition()
//                {
//                    GameName = "default game"
//                }, //default GameDefinition, without Goals(!) and Name
//            };

//            //generate Goals for default config without goals
//            var goalLocationsBlue = GenerateRandomPlaces(6, 0,
//                result.GameDefinition.BoardWidth, 0,
//                result.GameDefinition.GoalAreaLength
//            );
//            var goalLocationsRed = GenerateRandomPlaces(6, 0, result.GameDefinition.BoardWidth,
//                result.GameDefinition.GoalAreaLength + result.GameDefinition.TaskAreaLength,
//                result.GameDefinition.TaskAreaLength + 2 * result.GameDefinition.GoalAreaLength
//            );

//            result.GameDefinition.Goals = goalLocationsBlue.Select(location =>
//                new Shared.DTO.Configuration.GoalField
//                {
//                    Team = TeamColour.Blue,
//                    Type = GoalFieldType.Goal,
//                    X = location.x,
//                    Y = location.y
//                }
//            ).Concat(goalLocationsRed.Select(location =>
//                new Shared.DTO.Configuration.GoalField
//                {
//                    Team = TeamColour.Red,
//                    Type = GoalFieldType.Goal,
//                    X = location.x,
//                    Y = location.y
//                }
//            )).ToArray();

//            return result;
//        }
//        #endregion

//		public void Start()
//		{
//			Initialize();
//			RegisterPlayers();
//			CreatePlayers();
//			StartPlayers();
//			JoinPlayers();
//		}

//        public void Initialize()
//        {
//            var config = GenerateDefaultConfig();
//            if(BigCosts)
//                config.ActionCosts = new GameMasterSettingsActionCosts
//                {
//                    DiscoverDelay = 1000,
//                    KnowledgeExchangeDelay = 1000,
//                    MoveDelay = 1000,
//                    PickUpDelay = 1000,
//                    PlacingDelay = 1000,
//                    TestDelay = 1000
//                };

//            GameMaster = new BlockingGameMaster( config, new BoardComponentFactory() );

//            GameMaster.Log += (s, e) =>
//            {
//                Console.WriteLine(
//                    $"-------------------{e.Type,10} {e.Timestamp,20} {e.GameId,2} {e.PlayerId,3} {e.PlayerGuid.Substring(0, 4),5} {e.Colour,7} {e.Role,10}");
//            };

//            var registerGame = GameMaster.PerformConfirmGameRegistration();
//            registerGame.GameInfo[0].blueTeamPlayers = 1;
//            registerGame.GameInfo[0].redTeamPlayers = 1;
//            GameInfo = registerGame.GameInfo[0];

//            BluePlayers = new PlayerInGame[GameInfo.blueTeamPlayers];
//            RedPlayers = new PlayerInGame[GameInfo.redTeamPlayers];
//            BlueConfirms = new Shared.DTO.Communication.ConfirmJoiningGame[GameInfo.blueTeamPlayers];
//            RedConfirms = new Shared.DTO.Communication.ConfirmJoiningGame[GameInfo.redTeamPlayers];
//            BlueThreads = new Thread[GameInfo.blueTeamPlayers];
//            RedThreads = new Thread[GameInfo.redTeamPlayers];
//        }

//        public void RegisterPlayers()
//        {
//            // blue registration
//            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
//            {
//                BlueConfirms[i] = (Shared.DTO.Communication.ConfirmJoiningGame)GameMaster.PerformJoinGame(new Shared.DTO.Communication.JoinGame
//                {
//                    GameName = GameInfo.gameName,
//                    PlayerIdSpecified = false,
//                    PreferredRole = i == 0 ? PlayerRole.Leader : PlayerRole.Member,
//                    PreferredTeam = TeamColour.Blue
//                });
//            }

//            // red registration
//            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
//            {
//                RedConfirms[i] = (Shared.DTO.Communication.ConfirmJoiningGame)GameMaster.PerformJoinGame(new Shared.DTO.Communication.JoinGame
//                {
//                    GameName = GameInfo.gameName,
//                    PlayerIdSpecified = false,
//                    PreferredRole = i == 0 ? PlayerRole.Leader : PlayerRole.Member,
//                    PreferredTeam = TeamColour.Red
//                });
//            }
//        }

//        public void CreatePlayers()
//        {
//            // blue player create
//            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
//            {
//                var myConfirm = BlueConfirms[i];
//                var game = GameMaster.GetGame(myConfirm.PrivateGuid);
//                EndGame.Add(myConfirm.PlayerId, false);
//                /*BluePlayers[i] = new PlayerInGame(GameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
//                    (s, a) =>
//                    {
//                        lock (EndGame)
//                        {
//                            EndGame[myConfirm.playerId] = true;
//                        }
//                    });*/
//                BlueThreads[i] = new Thread(PlayerThread);
//                if(ShowData) BluePlayers[i].State.ReceiveDataLog += (s, e) => Console.WriteLine(e);
//            }

//            // red player create
//            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
//            {
//                var myConfirm = RedConfirms[i];
//                var game = GameMaster.GetGame(myConfirm.PrivateGuid);
//                EndGame.Add(myConfirm.PlayerId, false);
//                /*RedPlayers[i] = new PlayerInGame(GameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
//                    (s, a) =>
//                    {
//                        lock (EndGame)
//                        {
//                            EndGame[myConfirm.playerId] = true;
//                        }
//                    });*/
//                RedThreads[i] = new Thread(PlayerThread);
//                if (ShowData) RedPlayers[i].State.ReceiveDataLog += (s, e) => Console.WriteLine(e);
//            }
//        }

//        public void StartPlayers()
//        {
//            // blue player create
//            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
//            {
//                var myConfirm = BlueConfirms[i];
//                BlueThreads[i].Start(new PlayerThreadArgs
//                {
//                    id = myConfirm.PlayerId,
//                    player = BluePlayers[i]
//                });
//            }

//            // red player create
//            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
//            {
//                var myConfirm = RedConfirms[i];
//                RedThreads[i].Start(new PlayerThreadArgs
//                {
//                    id = myConfirm.PlayerId,
//                    player = RedPlayers[i]
//                });
//            }
//        }

//        public void JoinPlayers()
//        {
//            // blue wait
//            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
//            {
//                BlueThreads[i].Join();
//            }

//            // red wait
//            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
//            {
//                RedThreads[i].Join();
//            }
//        }

//        void PlayerThread(object object_args)
//        {
//            PlayerThreadArgs args = object_args as PlayerThreadArgs;
//            bool endGame;
//            do
//            {
//                args.player.State.ReceiveData(args.player.PerformAction());

//                lock (EndGame)
//                {
//                    endGame = EndGame[args.id];
//                }

//            } while (!endGame);
//        }
//        private class PlayerThreadArgs
//        {
//            public PlayerInGame player;
//            public ulong id;
//        }
//    }
//}