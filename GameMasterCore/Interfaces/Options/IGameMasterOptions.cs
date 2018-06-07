using Shared.DTOs.Configuration;
using Shared.Interfaces.Options;

namespace GameMasterCore.Interfaces.Options
{
	public interface IGameMasterOptions : IOptions
	{
		string Address { get; }
		GameMasterSettings Conf { get; }
	}
}
