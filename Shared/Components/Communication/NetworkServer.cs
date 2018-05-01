using Shared.Base.Communication;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Net.Sockets;
using System.Threading;

namespace Shared.Components.Communication
{
	public class NetworkServer : NetworkServerBase
	{
		#region NetworkServerBase
		public override void Accept( Action<INetworkClient> callback )
		{
			accept.Reset();
			Listener.BeginAcceptTcpClient( new AsyncCallback( OnAccept ), callback );
			accept.WaitOne();
		}
		#endregion
		#region NetworkServer
		private ManualResetEvent accept = new ManualResetEvent( false );
		public NetworkServer( TcpListener listener, INetworkFactory factory ) : base( listener, factory ) => listener.Start();
		protected void OnAccept( IAsyncResult ar )
		{
			Console.WriteLine( $"NetworkServer.OnAccept on { Thread.CurrentThread.ManagedThreadId }" );
			var client = Listener.EndAcceptTcpClient( ar );
			accept.Set();
			Console.WriteLine( "ACCEPT" );
			var callback = ar.AsyncState as Action<INetworkClient>;
			callback( new NetworkClient( client ) );
		}
		#endregion
	}
}
