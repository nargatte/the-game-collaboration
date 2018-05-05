using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Modules;
using PlayerCore.Interfaces.Players;
using PlayerCore.Interfaces.Proxies;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;
using System;

namespace PlayerCore.Base.Modules
{
	public abstract class PlayerModuleBase : ModuleBase, IPlayerModule
	{
		#region IPlayerModule
		public virtual PlayerSettings Configuration { get; }
		public virtual IPlayerFactory Factory { get; }
		public virtual IPlayer Player { get; }
		public virtual ICommunicationServerProxy Proxy { get; }
		#endregion
		#region PlayerModuleBase
		public PlayerModuleBase( int port, PlayerSettings configuration, IPlayerFactory factory ) : base( port )
		{
			Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			Player = Factory.CreatePlayer( Configuration.RetryJoinGameInterval );
			Proxy = Factory.CreateProxy();
		}
		#endregion
	}
}
