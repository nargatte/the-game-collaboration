using Shared.Components.Events;
using System;

namespace Shared.Interfaces
{
	public interface IStartable
	{
		void Start();
		event EventHandler<FinishArgs> Finish;
	}
}
