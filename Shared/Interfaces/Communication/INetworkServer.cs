using Shared.Interfaces.Factories;
using System;

namespace Shared.Interfaces.Communication
{
	public interface INetworkServer
	{
		INetworkFactory Factory { get; }
		void Accept( Action<INetworkClient> callback );
	}
}
