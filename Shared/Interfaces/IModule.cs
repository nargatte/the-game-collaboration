namespace Shared.Interfaces
{
	public interface IModule : IStartable
	{
		int Port { get; }
	}
}
