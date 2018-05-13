namespace Shared.Interfaces.Proxies
{
	public interface IServerProxy : IProxy
	{
		void UpdateLocal( IIdentity identity );
	}
}
