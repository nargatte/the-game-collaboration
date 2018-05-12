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
			RejectGameRegistration rejectGameRegistration;
			while( confirmGameRegistration is null )
			{
				System.Console.WriteLine( $"GAMEMASTER sends: { Shared.Components.Serialization.Serializer.Serialize( new RegisterGame() ) }." );
				await Proxy.SendAsync( new RegisterGame(), cancellationToken ).ConfigureAwait( false );
				while( true )
				{
					if( ( confirmGameRegistration = await Proxy.TryReceiveAsync<ConfirmGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						System.Console.WriteLine( $"GAMEMASTER received: { Shared.Components.Serialization.Serializer.Serialize( confirmGameRegistration ) }." );
						break;
					}
					else if( ( rejectGameRegistration = await Proxy.TryReceiveAsync<RejectGameRegistration>( cancellationToken ).ConfigureAwait( false ) ) != null )
					{
						System.Console.WriteLine( $"GAMEMASTER received: { Shared.Components.Serialization.Serializer.Serialize( rejectGameRegistration ) }." );
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
