using GameMasterCore.Base.Modules;
using GameMasterCore.Interfaces.Factories;
using Shared.DTOs.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace GameMasterCore.Components.Modules
{
    public class GameMasterModule : GameMasterModuleBase
    {
        #region IGameMasterModule
        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            GameMaster.Proxy = Factory.CreateProxy(Ip, Port, Configuration.KeepAliveInterval);
            await GameMaster.RunAsync(cancellationToken).ConfigureAwait(false);
        }
        #endregion
        #region GameMasterModule
        public GameMasterModule(string ip, int port, GameMasterSettings configuration, IGameMasterFactory factory) : base(ip, port, configuration, factory)
        {
        }
        #endregion
    }
}
