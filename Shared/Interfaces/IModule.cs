using Shared.Components.Events;
using Shared.Interfaces.Modules;
using System;

namespace Shared.Interfaces
{
	public interface IModule : IStartable
	{
		int Port { get; }
		event EventHandler<ExitArgs> Exit;
	}
}
