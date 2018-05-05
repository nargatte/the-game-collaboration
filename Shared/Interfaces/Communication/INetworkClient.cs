using Shared.Interfaces.Factories;
using System;

namespace Shared.Interfaces.Communication
{
	public interface INetworkClient : IDisposable
	{
		INetworkFactory Factory { get; }
	}
}
