using GameMasterCore.Base.GameMasters;
using Shared.Components.Factories;
using Shared.Components.Tasks;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using Shared.Messages.Communication;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.GameMasters
{
    public class GameMaster : GameMasterBase
    {
        #region GameMasterBase
        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //System.Console.WriteLine( $"GameMaster sends: { Shared.Components.Serialization.Serializer.Serialize( new RegisterGame() ) }." );
            await Proxy.SendAsync(
                new Shared.DTOs.Communication.RegisterGame()
                {
                    NewGameInfo = new Shared.DTOs.Communication.GameInfo()
                    {
                        BlueTeamPlayers = GameDefinition.NumberOfPlayersPerTeam,
                        RedTeamPlayers = GameDefinition.NumberOfPlayersPerTeam,
                        GameName = GameDefinition.GameName
                    }
                }, cancellationToken).ConfigureAwait(false);
            //TODO: weź odpowiedź od serwera i sprawdź game id

            await Task.Run(async () => await Listener(cancellationToken).ConfigureAwait(false));
        }
        #endregion
        #region GameMaster
        public GameMaster(GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval) : base(gameDefinition, actionCosts, retryRegisterGameInterval)
        {
            initTmpInnerGM(gameDefinition, actionCosts);
        }
        #endregion

        BlockingGameMaster innerGM;
        List<Task<GameMessage>> tasks;

        void initTmpInnerGM(GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts)
        {
            innerGM = new BlockingGameMaster(new GameMasterSettings()
            {
                ActionCosts = new GameMasterSettingsActionCosts() { DiscoverDelay = 0, KnowledgeExchangeDelay = 0, MoveDelay = 0, PickUpDelay = 0, PlacingDelay = 0, TestDelay = 0 },
                GameDefinition = gameDefinition
            }, new BoardComponentFactory());
        }

        async Task Listener(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            while (innerGM.playerGuidToId.Keys.Count != GameDefinition.NumberOfPlayersPerTeam * 2)
            {
                cancellationToken.ThrowIfCancellationRequested();
                JoinGame joinGame;
                if ((joinGame = await Proxy.TryReceiveAsync<JoinGame>(cancellationToken).ConfigureAwait(false)) != null)
                {
                    //Console.WriteLine($"GM receives: { Shared.Components.Serialization.Serializer.Serialize(joinGame) }.");
                    await Proxy.SendAsync(innerGM.PerformJoinGame(joinGame), cancellationToken);
                }
                else
                    Proxy.Discard();
            }
            foreach (var player in innerGM.playerGuidToId)
            {
                await Proxy.SendAsync(innerGM.GetGame(player.Key), cancellationToken);
            }
            TaskManager taskPlacer = new TaskManager(async(ct) => innerGM.PerformCreatePieceAndPlaceRandomly(), GameDefinition.PlacingNewPiecesFrequency, true, cancellationToken);
            taskPlacer.Start();
            //var taskPlacer = Task.Run(async () => await PiecePlacer(cancellationToken).ConfigureAwait(false));
            var taskPerformer = Task.Run(async () => await TaskPerformer(cancellationToken).ConfigureAwait(false));

            Exception ex = null;
            try
            {
                while (true)
                {
                    Move moveRequest;
                    Discover discoverRequest;
                    PickUpPiece pickUpRequest;
                    TestPiece testPieceRequest;
                    PlacePiece placeRequest;
                    //+knowledge exchange
                    if ((moveRequest = await Proxy.TryReceiveAsync<Move>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        tasks.Add(DelayMessage(moveRequest, ActionCosts.MoveDelay, cancellationToken));
                    }
                    else if ((discoverRequest = await Proxy.TryReceiveAsync<Discover>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        tasks.Add(DelayMessage(discoverRequest, ActionCosts.DiscoverDelay, cancellationToken));
                    }
                    else if ((pickUpRequest = await Proxy.TryReceiveAsync<PickUpPiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        tasks.Add(DelayMessage(pickUpRequest, ActionCosts.PickUpDelay, cancellationToken));
                    }
                    else if ((testPieceRequest = await Proxy.TryReceiveAsync<TestPiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        tasks.Add(DelayMessage(testPieceRequest, ActionCosts.TestDelay, cancellationToken));
                    }
                    else if ((placeRequest = await Proxy.TryReceiveAsync<PlacePiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        tasks.Add(DelayMessage(placeRequest, ActionCosts.PlacingDelay, cancellationToken));
                    }
                    else
                        Proxy.Discard();
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                ex = e;
            }
            finally
            {
                try
                {
                    taskPlacer.Stop();
                }
                catch (OperationCanceledException)
                {
                    if (ex != null)
                        throw new AggregateException(ex);
                }
                catch (Exception e)
                {
                    throw ex is null ? new AggregateException(e) : new AggregateException(ex, e);
                }

                try
                {
                    await taskPerformer;
                }
                catch (OperationCanceledException)
                {
                    if (ex != null)
                        throw new AggregateException(ex);
                }
                catch (Exception e)
                {
                    throw ex is null ? new AggregateException(e) : new AggregateException(ex, e);
                }

                try
                {
                    await Task.WhenAll(tasks.ToArray());
                }
                catch (OperationCanceledException)
                {
                    if (ex != null)
                        throw new AggregateException(ex);
                }
                catch (Exception e)
                {
                    throw ex is null ? new AggregateException(e) : new AggregateException(ex, e);
                }

            }
            
        }

        async Task TaskPerformer(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            while (true)
            {
                Task<GameMessage> task = await Task.WhenAny(tasks);
                GameMessage message = await task;
                await Proxy.SendAsync(innerGM.Perform(message), cancellationToken);
            }
        }

        async Task<GameMessage> DelayMessage(GameMessage message, uint delay, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(TimeSpan.FromMilliseconds(delay), cancellationToken).ConfigureAwait(false);
            return message;
        }
    }
}
