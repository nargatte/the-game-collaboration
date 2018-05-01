using System;
using System.Net.Sockets;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;

namespace Shared.Base.Communication
{
	public abstract class NetworkServerBase : INetworkServer
	{
		#region INetworkServer
		public virtual INetworkFactory Factory { get; }
		public abstract void Accept( Action<INetworkClient> callback );
		#endregion
		#region NetworkServerBase
		protected TcpListener Listener { get; }
		protected NetworkServerBase( TcpListener listener, INetworkFactory factory )
		{
			Listener = listener;
			Factory = factory;
		}
		#endregion
	}
}
