using PlayerCore.Interfaces.Factories;
using PlayerCore.Interfaces.Players;
using Shared.DTO.Configuration;
using Shared.Enums;
using Shared.Interfaces.Modules;

namespace PlayerCore.Interfaces.Modules
{
	public interface IPlayerModule : IModule
	{
		PlayerSettings Configuration { get; }
		string GameName { get; }
		TeamColour Team { get; }
		PlayerRole Role { get; }
		IPlayerFactory Factory { get; }
		IPlayer Player { get; }
	}
}
