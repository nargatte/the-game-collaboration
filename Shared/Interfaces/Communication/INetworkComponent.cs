using Shared.Interfaces.Factories;
using System;

namespace Shared.Interfaces.Communication
{
	public interface INetworkComponent : IDisposable
	{
		INetworkFactory Factory { get; }
	}
}
