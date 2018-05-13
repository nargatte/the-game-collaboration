using GameMasterCore.Base.GameMasters;
using Shared.DTOs.Communication;
using Shared.DTOs.Configuration;
using Shared.Enums;
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
			while( Proxy.Local.Id == 0uL )
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
					ConfirmGameRegistration confirmGameRegistration;
					RejectGameRegistration rejectGameRegistration;
					if( ( confirmGameRegistration = await Proxy.TryReceiveAsync<ConfirmGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						PerformConfirmGameRegistration( confirmGameRegistration, cancellationToken );
						break;
					}
					else if( ( rejectGameRegistration = await Proxy.TryReceiveAsync<RejectGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						await Task.Delay( TimeSpan.FromMilliseconds( RetryRegisterGameInterval ), cancellationToken ).ConfigureAwait( false );
						break;
					}
					else
						Proxy.Discard();
				}
			}
		}
		#endregion
		private ulong id;
        #region GameMaster
        public GameMaster( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval ) : base( gameDefinition, actionCosts, retryRegisterGameInterval )
        {
        }
		protected void PerformConfirmGameRegistration( ConfirmGameRegistration confirmGameRegistration, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			id = confirmGameRegistration.GameId;
			Proxy.UpdateLocal( Proxy.Factory.CreateIdentity( HostType.GameMaster, id ) );
		}
		#endregion
	}
}
