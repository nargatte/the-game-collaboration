using GameMasterCore.Base.GameMasters;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.GameMasters
{
	public class GameMaster : GameMasterBase
    {
        #region GameMasterBase
        public override async Task RunAsync( CancellationToken cancellationToken )
        {
            cancellationToken.ThrowIfCancellationRequested();
			ConfirmGameRegistration confirmGameRegistration = null;
			while( confirmGameRegistration is null )
			{
				var registerGame = new RegisterGame
				{
					NewGameInfo = new GameInfo
					{
						GameName = GameDefinition.GameName,
						RedTeamPlayers = GameDefinition.NumberOfPlayersPerTeam,
						BlueTeamPlayers = GameDefinition.NumberOfPlayersPerTeam
					}
				};
				await Proxy.SendAsync( registerGame, cancellationToken ).ConfigureAwait( false );
				while( true )
				{
					RejectGameRegistration rejectGameRegistration;
					if( ( confirmGameRegistration = await Proxy.TryReceiveAsync<ConfirmGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						break;
					}
					else if( ( rejectGameRegistration = await Proxy.TryReceiveAsync<RejectGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						break;
					}
					else
						Proxy.Discard();
				}
				await Task.Delay( TimeSpan.FromMilliseconds( RetryRegisterGameInterval ), cancellationToken ).ConfigureAwait( false );
			}
		}
        #endregion
        #region GameMaster
        public GameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval ) : base( gameDefinition, actionCosts, retryRegisterGameInterval )
        {
        }
        #endregion
    }
}
