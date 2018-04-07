﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameMasterCore;
using Shared.Components.Players;
using Shared.Messages.Configuration;
using PlayerCore;
using Shared.Enums;
using Shared.Messages.Communication;

namespace CommunicationSubstitute
{
    class Program
    {

        static void Main(string[] args)
        {
            var gameMaster = new BlockingGameMaster();
            var registerGame = gameMaster.PerformConfirmGameRegistration();
            var gameInfo = registerGame.GameInfo[0];

            PlayerInGame[] bluePlayers = new PlayerInGame[gameInfo.blueTeamPlayers];
            PlayerInGame[] redPlayers = new PlayerInGame[gameInfo.redTeamPlayers];
            ConfirmJoiningGame[] blueConfirms = new ConfirmJoiningGame[gameInfo.blueTeamPlayers];
            ConfirmJoiningGame[] redConfirms = new ConfirmJoiningGame[gameInfo.redTeamPlayers];
            Thread[] blueThreads = new Thread[gameInfo.blueTeamPlayers];
            Thread[] redThreads = new Thread[gameInfo.redTeamPlayers];

            // blue registration
            for (ulong i = 0; i < gameInfo.blueTeamPlayers; i++)
            {
                blueConfirms[i] = (ConfirmJoiningGame) gameMaster.PerformJoinGame(new JoinGame
                {
                    gameName = gameInfo.gameName,
                    playerIdSpecified = false,
                    preferredRole = i==0?PlayerType.Leader:PlayerType.Member,
                    preferredTeam = TeamColour.Blue
                });
            }

            // red registration
            for (ulong i = 0; i < gameInfo.redTeamPlayers; i++)
            {
                redConfirms[i] = (ConfirmJoiningGame)gameMaster.PerformJoinGame(new JoinGame
                {
                    gameName = gameInfo.gameName,
                    playerIdSpecified = false,
                    preferredRole = i == 0 ? PlayerType.Leader : PlayerType.Member,
                    preferredTeam = TeamColour.Red
                });
            }

            // blue player create
            for (ulong i = 0; i < registerGame.GameInfo[0].blueTeamPlayers; i++)
            {
                var myConfirm = blueConfirms[i];
                var game = gameMaster.game[myConfirm.playerId];
                EndGame.Add(myConfirm.playerId, false);
                bluePlayers[i] = new PlayerInGame(gameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
                    (s, a) =>
                    {
                        lock (EndGame)
                        {
                            EndGame[myConfirm.playerId] = true;
                        }
                    });
                blueThreads[i] = new Thread(() => PlayerThread(bluePlayers[i], myConfirm.playerId));
            }

            // red player create
            for (ulong i = 0; i < registerGame.GameInfo[0].redTeamPlayers; i++)
            {
                var myConfirm = redConfirms[i];
                var game = gameMaster.game[myConfirm.playerId];
                EndGame.Add(myConfirm.playerId, false);
                redPlayers[i] = new PlayerInGame(gameMaster, game, myConfirm.playerId, myConfirm.privateGuid, myConfirm.gameId,
                    (s, a) =>
                    {
                        lock (EndGame)
                        {
                            EndGame[myConfirm.playerId] = true;
                        }
                    });
                redThreads[i] = new Thread(() => PlayerThread(bluePlayers[i], myConfirm.playerId));
            }

            // blue wait
            for (ulong i = 0; i < registerGame.GameInfo[0].blueTeamPlayers; i++)
            {
                blueThreads[i].Join();
            }

            // red wait
            for (ulong i = 0; i < registerGame.GameInfo[0].redTeamPlayers; i++)
            {
                redThreads[i].Join();
            }
        }

        static readonly Dictionary<ulong, bool> EndGame =  new Dictionary<ulong, bool>();

        private static void PlayerThread(PlayerInGame player, ulong id)
        {
            bool endGame;
            do
            {
                player.PerformAction();

                lock (EndGame)
                {
                    endGame = EndGame[id];
                }
            } while (endGame);
        }
    }
}