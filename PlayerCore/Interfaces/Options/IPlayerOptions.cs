using Shared.DTO.Configuration;
using Shared.Enums;
using Shared.Interfaces.Options;

namespace PlayerCore.Interfaces.Options
{
	public interface IPlayerOptions : IOptions
	{
		string Address { get; }
		PlayerSettings Conf { get; }
		string Game { get; }
		TeamColour Team { get; }
		PlayerRole Role { get; }
	}
}
