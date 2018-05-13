using Shared.Enums;

namespace Shared.Interfaces.Proxies
{
	public interface IIdentity
	{
		HostType Type { get; }
		ulong Id { get; }
	}
}
