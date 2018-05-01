using Shared.DTOs.Configuration;
using Shared.Interfaces.Modules;

namespace PlayerCore.Interfaces
{
	public interface IPlayerModule : IModule
	{
		PlayerSettings Configuration { get; }
		IPlayer Player { get; }
	}
}
