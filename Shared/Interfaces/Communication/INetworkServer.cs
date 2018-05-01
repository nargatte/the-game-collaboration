using System;

namespace Shared.Interfaces.Communication
{
	public interface INetworkServer
	{
		void Accept( Action<INetworkClient> callback );
	}
}
