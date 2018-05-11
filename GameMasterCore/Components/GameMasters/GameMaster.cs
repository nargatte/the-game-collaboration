using GameMasterCore.Base.GameMasters;
using Shared.Components.Factories;
using Shared.Components.Tasks;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using Shared.Messages.Communication;
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
            var taskPlacer = Task.Run(async () => await PiecePlacer(cancellationToken).ConfigureAwait(false));
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
                        new TaskDelayer(async (ct) => await Proxy.SendAsync(innerGM.PerformMove(moveRequest), ct), ActionCosts.MoveDelay, cancellationToken);
                    }
                    else if ((discoverRequest = await Proxy.TryReceiveAsync<Discover>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        new TaskDelayer(async (ct) => await Proxy.SendAsync(innerGM.PerformDiscover(discoverRequest), ct), ActionCosts.DiscoverDelay, cancellationToken);
                    }
                    else if ((pickUpRequest = await Proxy.TryReceiveAsync<PickUpPiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        //todo: taskdelayer
                        await Proxy.SendAsync(innerGM.PerformPickUp(pickUpRequest), cancellationToken);
                    }
                    else if ((testPieceRequest = await Proxy.TryReceiveAsync<TestPiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        await Proxy.SendAsync(innerGM.PerformTestPiece(testPieceRequest), cancellationToken);
                    }
                    else if ((placeRequest = await Proxy.TryReceiveAsync<PlacePiece>(cancellationToken).ConfigureAwait(false)) != null)
                    {
                        await Proxy.SendAsync(innerGM.PerformPlace(placeRequest), cancellationToken);
                    }
                    else
                        Proxy.Discard();
                }
            }
            finally
            {
                await taskPlacer;
            }
            
        }

        async Task PiecePlacer(CancellationToken cancellationToken)
        {

        }
    }
}
