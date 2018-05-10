using GameMasterCore.Interfaces.Factories;
using GameMasterCore.Interfaces.GameMasters;
using GameMasterCore.Interfaces.Modules;
using Shared.Base.Modules;
using Shared.DTOs.Configuration;
using System;

namespace GameMasterCore.Base.Modules
{
	public abstract class GameMasterModuleBase : ModuleBase, IGameMasterModule
    {
        #region IGameMasterModule
        public virtual GameMasterSettings Configuration { get; }
        public virtual IGameMasterFactory Factory { get; }
        public virtual IGameMaster GameMaster { get; }
        #endregion
        #region GameMasterModuleBase
        public GameMasterModuleBase( string ip, int port, GameMasterSettings configuration, IGameMasterFactory factory ) : base( ip, port )
        {
            Configuration = configuration is null ? throw new ArgumentNullException( nameof( configuration ) ) : configuration;
            Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
			GameMaster = Factory.CreateGameMaster( Configuration.GameDefinition, Configuration.ActionCosts, Configuration.RetryRegisterGameInterval );
			if( GameMaster is null )
				throw new NotImplementedException( nameof( Factory ) );
		}
        #endregion
    }
}
