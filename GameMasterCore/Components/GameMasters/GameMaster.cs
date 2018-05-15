using GameMasterCore.Base.GameMasters;
using Shared.Components.Factories;
using Shared.Components.Tasks;
using Shared.Const;
using Shared.DTOs.Configuration;
using Shared.Enums;
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
			while( Proxy.Local.Id == ConstHelper.AnonymousId )
			{
				var registerGame = new Shared.DTOs.Communication.RegisterGame
                {
					NewGameInfo = new Shared.DTOs.Communication.GameInfo
                    {
						GameName = GameDefinition.GameName,
						RedTeamPlayers = GameDefinition.NumberOfPlayersPerTeam,
						BlueTeamPlayers = GameDefinition.NumberOfPlayersPerTeam
					}
				};
				await Proxy.SendAsync( registerGame, cancellationToken ).ConfigureAwait( false );
				while( true )
				{
                    Shared.DTOs.Communication.ConfirmGameRegistration confirmGameRegistration;
                    Shared.DTOs.Communication.RejectGameRegistration rejectGameRegistration;
					if( ( confirmGameRegistration = await Proxy.TryReceiveAsync<Shared.DTOs.Communication.ConfirmGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						PerformConfirmGameRegistration( confirmGameRegistration, cancellationToken );
						break;
					}
					else if( ( rejectGameRegistration = await Proxy.TryReceiveAsync<Shared.DTOs.Communication.RejectGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						await Task.Delay( TimeSpan.FromMilliseconds( RetryRegisterGameInterval ), cancellationToken ).ConfigureAwait( false );
						break;
					}
					else
						Proxy.Discard();
				}
			}
            await Task.Run(async () => await Listener(cancellationToken).ConfigureAwait(false));
        }
		#endregion
		private ulong id;
        #region GameMaster
        public GameMaster(GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval) : base(gameDefinition, actionCosts, retryRegisterGameInterval)
        {
            initTmpInnerGM(gameDefinition, actionCosts);
        }
		protected void PerformConfirmGameRegistration(Shared.DTOs.Communication.ConfirmGameRegistration confirmGameRegistration, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			id = confirmGameRegistration.GameId;
            Proxy.UpdateLocal( Proxy.Factory.CreateIdentity( HostType.GameMaster, id ) );

            //temp
            innerGM.gameId = confirmGameRegistration.GameId;
        }
		#endregion

        BlockingGameMaster innerGM;
        List<Task<GameMessage>> tasks = new List<Task<GameMessage>>();

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
                Shared.DTOs.Communication.JoinGame joinGame;
                if ((joinGame = await Proxy.TryReceiveAsync<Shared.DTOs.Communication.JoinGame>(cancellationToken).ConfigureAwait(false)) != null)
                {
                    //Console.WriteLine($"GM receives: { Shared.Components.Serialization.Serializer.Serialize(joinGame) }.");
                    Shared.DTOs.Communication.PlayerMessage message = innerGM.PerformJoinGame(joinGame);
                    if(message is Shared.DTOs.Communication.RejectJoiningGame)
                        await Proxy.SendAsync(message as Shared.DTOs.Communication.RejectJoiningGame, cancellationToken);
                    else
                        await Proxy.SendAsync(message as Shared.DTOs.Communication.ConfirmJoiningGame, cancellationToken);

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
