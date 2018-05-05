using PlayerCore.Interfaces.Players;
using Shared.DTOs.Configuration;
using Shared.Interfaces.Modules;

namespace PlayerCore.Interfaces.Modules
{
	public interface IPlayerModule : IModule
	{
		PlayerSettings Configuration { get; }
		IPlayer Player { get; }
	}
}
