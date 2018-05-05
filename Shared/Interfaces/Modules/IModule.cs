namespace Shared.Interfaces.Modules
{
	public interface IModule : IRunnable
	{
		int Port { get; }
	}
}