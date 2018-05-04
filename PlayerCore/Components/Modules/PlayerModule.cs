using PlayerCore.Interfaces;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlayerCore.Components.Modules
{
	public class PlayerModule : ModuleBase, IPlayerModule
	{
		#region IPlayerModule
		public virtual PlayerSettings Configuration { get; }
		public virtual IPlayer Player { get; protected set; }
		/*public override void Start()
		{
			try
			{
				//CommunicationServer = new CommunicationServer( Port, Configuration );
				//CommunicationServer.Start();
				OnFinish();
			}
			catch( Exception e )
			{
				OnFinish( e );
			}
		}*/

		public override Task RunAsync( CancellationToken ct )
		{
			Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: starting player module" );
			return Task.CompletedTask;
		}
		#endregion
		#region PlayerModule
		public PlayerModule( int port, PlayerSettings configuration ) : base( port ) => Configuration = configuration;
		#endregion
	}
}
