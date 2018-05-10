using GameMasterCore.Interfaces.GameMasters;
using Shared.DTOs.Configuration;
using Shared.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Base.GameMasters
{
	public abstract class GameMasterBase : IGameMaster
    {
        #region IGameMaster
        public abstract Task RunAsync( CancellationToken cancellationToken );
        public virtual GameMasterSettingsGameDefinition GameDefinition { get; }
		public virtual GameMasterSettingsActionCosts ActionCosts { get; }
		public virtual uint RetryRegisterGameInterval { get; }
		public virtual IServerProxy Proxy { get; set; }
        #endregion
        #region GameMasterBase
        protected GameMasterBase( GameMasterSettingsGameDefinition gameDefinition, GameMasterSettingsActionCosts actionCosts, uint retryRegisterGameInterval )
        {
			GameDefinition = gameDefinition;
			ActionCosts = actionCosts;
			RetryRegisterGameInterval = retryRegisterGameInterval;
		}
        #endregion
    }
}
