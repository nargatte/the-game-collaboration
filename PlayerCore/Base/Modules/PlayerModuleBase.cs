﻿using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Modules;
using PlayerCore.Interfaces.Players;
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
		#endregion
		#region PlayerModuleBase
		public PlayerModuleBase( string ip, int port, PlayerSettings configuration, IPlayerFactory factory ) : base( ip, port )
		{
			Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
			Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			Player = Factory.CreatePlayer( Configuration.RetryJoinGameInterval );
		}
		#endregion
	}
}
