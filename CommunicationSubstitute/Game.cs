using System;
using System.Collections.Generic;
using System.Threading;
using GameMasterCore;
using PlayerCore;
using Shared.Enums;
using Shared.Messages.Communication;

namespace CommunicationSubstitute
{
    public class Game
    {
        public BlockingGameMaster GameMaster;

        public GameInfo GameInfo;

        public PlayerInGame[] BluePlayers;
        public PlayerInGame[] RedPlayers;
        public ConfirmJoiningGame[] BlueConfirms;
        public ConfirmJoiningGame[] RedConfirms;
        public Thread[] BlueThreads;
        public Thread[] RedThreads;

        readonly Dictionary<ulong, bool> EndGame = new Dictionary<ulong, bool>();

        public void Initialize()
        {
            GameMaster = new BlockingGameMaster();

            GameMaster.Log += (s, e) =>
            {
                Console.WriteLine(
                    $" {e.Type,10} {e.Timestamp,20} {e.GameId,2} {e.PlayerId,3} {e.PlayerGuid.Substring(0, 4),5} {e.Colour,7} {e.Role,10}");
            };

            var registerGame = GameMaster.PerformConfirmGameRegistration();
            GameInfo = registerGame.GameInfo[0];

            BluePlayers = new PlayerInGame[GameInfo.blueTeamPlayers];
            RedPlayers = new PlayerInGame[GameInfo.redTeamPlayers];
            BlueConfirms = new ConfirmJoiningGame[GameInfo.blueTeamPlayers];
            RedConfirms = new ConfirmJoiningGame[GameInfo.redTeamPlayers];
            BlueThreads = new Thread[GameInfo.blueTeamPlayers];
            RedThreads = new Thread[GameInfo.redTeamPlayers];
        }

        public void RegisterPlayers()
        {
            // blue registration
            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
            {
                BlueConfirms[i] = (ConfirmJoiningGame)GameMaster.PerformJoinGame(new JoinGame
                {
                    gameName = GameInfo.gameName,
                    playerIdSpecified = false,
                    preferredRole = i == 0 ? PlayerType.Leader : PlayerType.Member,
                    preferredTeam = TeamColour.Blue
                });
            }

            // red registration
            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
            {
                RedConfirms[i] = (ConfirmJoiningGame)GameMaster.PerformJoinGame(new JoinGame
                {
                    gameName = GameInfo.gameName,
                    playerIdSpecified = false,
                    preferredRole = i == 0 ? PlayerType.Leader : PlayerType.Member,
                    preferredTeam = TeamColour.Red
                });
            }
        }

        public void CreatePlayers()
        {
            // blue player create
            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
            {
                var myConfirm = BlueConfirms[i];
                var game = GameMaster.GetGame(myConfirm.privateGuid);
                EndGame.Add(myConfirm.playerId, false);
                BluePlayers[i] = new PlayerInGame(GameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
                    (s, a) =>
                    {
                        lock (EndGame)
                        {
                            EndGame[myConfirm.playerId] = true;
                        }
                    });
                BlueThreads[i] = new Thread(PlayerThread);
            }

            // red player create
            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
            {
                var myConfirm = RedConfirms[i];
                var game = GameMaster.GetGame(myConfirm.privateGuid);
                EndGame.Add(myConfirm.playerId, false);
                RedPlayers[i] = new PlayerInGame(GameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
                    (s, a) =>
                    {
                        lock (EndGame)
                        {
                            EndGame[myConfirm.playerId] = true;
                        }
                    });
                RedThreads[i] = new Thread(PlayerThread);
            }
        }

        public void StartPlayers()
        {
            // blue player create
            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
            {
                var myConfirm = BlueConfirms[i];
                BlueThreads[i].Start(new PlayerThreadArgs
                {
                    id = myConfirm.playerId,
                    player = BluePlayers[i]
                });
            }

            // red player create
            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
            {
                var myConfirm = RedConfirms[i];
                RedThreads[i].Start(new PlayerThreadArgs
                {
                    id = myConfirm.playerId,
                    player = RedPlayers[i]
                });
            }
        }

        public void JoinPlayers()
        {
            // blue wait
            for (ulong i = 0; i < GameInfo.blueTeamPlayers; i++)
            {
                BlueThreads[i].Join();
            }

            // red wait
            for (ulong i = 0; i < GameInfo.redTeamPlayers; i++)
            {
                RedThreads[i].Join();
            }
        }

        void PlayerThread(object object_args)
        {
            PlayerThreadArgs args = object_args as PlayerThreadArgs;
            bool endGame;
            do
            {
                args.player.State.ReceiveData(args.player.PerformAction());

                lock (EndGame)
                {
                    endGame = EndGame[args.id];
                }

            } while (!endGame);
        }
        private class PlayerThreadArgs
        {
            public PlayerInGame player;
            public ulong id;
        }
    }
}