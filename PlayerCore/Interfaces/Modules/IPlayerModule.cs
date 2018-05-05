using PlayerCore.Interfaces.Players;
using PlayerCore.Interfaces.Proxies;
using Shared.DTOs.Configuration;
using Shared.Interfaces.Modules;

namespace PlayerCore.Interfaces.Modules
{
	public interface IPlayerModule : IModule
	{
		PlayerSettings Configuration { get; }
		IPlayer Player { get; }
		ICommunicationServerProxy Proxy { get; }
	}
}
