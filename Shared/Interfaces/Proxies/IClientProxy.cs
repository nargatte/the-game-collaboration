namespace Shared.Interfaces.Proxies
{
	public interface IClientProxy : IProxy
	{
		void UpdateRemote( IIdentity identity );
	}
}
