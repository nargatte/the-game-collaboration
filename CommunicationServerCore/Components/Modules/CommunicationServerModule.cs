using CommunicationServerCore.Base.Modules;
using CommunicationServerCore.Interfaces.Factories;
using Shared.DTOs.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : CommunicationServerModuleBase
	{
		#region CommunicationServerModuleBase
		public override Task RunAsync( CancellationToken ct )
		{
			Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: starting communication server module" );
			//return Task.FromException( new NotImplementedException() );
			return Task.CompletedTask;
		}
		/*public override void Start()
		{
			try
			{
				CommunicationServer.Finish += ( s, e ) =>
				{
					if( e.IsSuccess )
						Console.WriteLine( "Communication server finished successfully." );
					else
						Console.WriteLine( $"Communication server finished with exception: { e.Exception }." );
				};
				CommunicationServer.Start();
				OnFinish();
			}
			catch( Exception e )
			{
				OnFinish( e );
			}
		}*/
		#endregion
		#region CommunicationServerModule
		public CommunicationServerModule( int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( port, configuration, factory )
		{
		}
		#endregion
	}
}
