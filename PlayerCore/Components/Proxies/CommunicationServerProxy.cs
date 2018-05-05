using PlayerCore.Base.Proxies;
using Shared.Interfaces.Factories;

namespace PlayerCore.Components.Proxies
{
	public class CommunicationServerProxy : CommunicationServerProxyBase
	{
		#region CommunicationServerProxy
		public CommunicationServerProxy( int port, uint keepAliveInterval, INetworkFactory factory ) : base( port, keepAliveInterval, factory )
		{
		}
		#endregion
	}
}
