using PlayerCore.Base.Modules;
using PlayerCore.Interfaces.Factories;
using Shared.DTOs.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Modules
{
	public class PlayerModule : PlayerModuleBase
	{
		#region IPlayerModule
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Player.RunAsync( cancellationToken ).ConfigureAwait( false );
		}
		#endregion
		#region PlayerModule
		public PlayerModule( int port, PlayerSettings configuration, IPlayerFactory factory ) : base( port, configuration, factory )
		{
		}
		#endregion
	}
}
