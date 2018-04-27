namespace Shared.Interfaces
{
	public interface IModule
	{
		int Port { get; }
		void Start();
	}
}
