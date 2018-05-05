using GameMasterCore.Interfaces.GameMasters;
using GameMasterCore.Interfaces.Proxies;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Base.GameMasters
{
    public abstract class GameMasterBase : IGameMaster
    {
        #region IGameMaster
        public abstract Task RunAsync(CancellationToken cancellationToken);
        public virtual uint RetryJoinGameInterval { get; }
        public virtual ICommunicationServerProxy Proxy { get; set; }
        #endregion
        #region GameMasterBase
        protected GameMasterBase()
        { }
        #endregion
    }
}
