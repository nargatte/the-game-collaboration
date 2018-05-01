using PlayerCore.Components.Players;
using PlayerCore.Interfaces;
using Shared.Base;
using Shared.DTOs.Configuration;
using System;

namespace PlayerCore.Components.Modules
{
	public class PlayerModule : Module, IPlayerModule
	{
		#region IPlayerModule
		public virtual PlayerSettings Configuration { get; }
		public virtual IPlayer Player { get; protected set; }
		public override void Start()
		{
			try
			{
				//CommunicationServer = new CommunicationServer( Port, Configuration );
				//CommunicationServer.Start();
				OnExit();
			}
			catch( Exception e )
			{
				OnExit( e );
			}
		}
		#endregion
		#region PlayerModule
		public PlayerModule( int port, PlayerSettings configuration ) : base( port ) => Configuration = configuration;
		#endregion
	}
}
