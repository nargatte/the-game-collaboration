namespace Shared.Interfaces.Modules
{
	public interface IModule : IStartable
	{
		int Port { get; }
	}
}