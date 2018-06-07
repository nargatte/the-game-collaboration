using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Modules;
using PlayerCore.Interfaces.Players;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;
using Shared.Enums;
using System;

namespace PlayerCore.Base.Modules
{
	public abstract class PlayerModuleBase : ModuleBase, IPlayerModule
	{
		#region IPlayerModule
		public virtual PlayerSettings Configuration { get; }
		public virtual string GameName { get; }
		public virtual TeamColour Team { get; }
		public virtual PlayerRole Role { get; }
		public virtual IPlayerFactory Factory { get; }
		public virtual IPlayer Player { get; }
		#endregion
		#region PlayerModuleBase
		protected PlayerModuleBase( string ip, int port, PlayerSettings configuration, string gameName, TeamColour team, PlayerRole role, IPlayerFactory factory ) : base( ip, port )
		{
			Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
			GameName = gameName is null ? throw new ArgumentNullException( nameof( gameName ) ) : gameName;
			Team = team;
			Role = role;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			Player = Factory.CreatePlayer( Configuration.RetryJoinGameInterval, GameName, Team, Role );
			if( Player is null )
				throw new NotImplementedException( nameof( Factory ) );
		}
		#endregion
	}
}
