using CommunicationServerCore.Base.Modules;
using CommunicationServerCore.Interfaces.Factories;
using Shared.DTOs.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : CommunicationServerModuleBase
	{
		#region CommunicationServerModuleBase
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			await CommunicationServer.RunAsync( cancellationToken ).ConfigureAwait( false );
		}
		#endregion
		#region CommunicationServerModule
		public CommunicationServerModule( string ip, int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( ip, port, configuration, factory )
		{
		}
		#endregion
	}
}
