using Shared.DTOs.Configuration;
using Shared.Interfaces.Options;

namespace CommunicationServerCore.Interfaces.Options
{
	public interface ICommunicationServerOptions : IOptions
	{
		CommunicationServerSettings Conf { get; }
	}
}
