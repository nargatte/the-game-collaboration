using Shared.DTOs.Configuration;
using Shared.Interfaces;
using Shared.Interfaces.Proxies;

namespace GameMasterCore.Interfaces.GameMasters
{
	public interface IGameMaster : IRunnable
    {
		GameMasterSettingsGameDefinition GameDefinition { get; }
		GameMasterSettingsActionCosts ActionCosts { get; }
		uint RetryRegisterGameInterval { get; }
		IServerProxy Proxy { get; set; }
    }
}