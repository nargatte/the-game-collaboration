using Shared.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Shared.Components.Communication
{
	public class NetworkServer : INetworkServer
	{
		#region INetworkServer
		public virtual int Port { get; }
		public virtual void Accept( Action<NetworkClient> callback )
		{
			accept.Reset();
			listener.BeginAcceptTcpClient( new AsyncCallback( OnAccept ), callback );
			accept.WaitOne();
		}
		#endregion
		#region NetworkServer
		private TcpListener listener;
		private ManualResetEvent accept = new ManualResetEvent( false );
		public NetworkServer( int port )
		{
			Port = port;
			Initialize();
		}
		protected void Initialize()
		{
			listener = new TcpListener( IPAddress.Parse( "127.0.0.1" ), Port );
			listener.Start();
		}
		protected void OnAccept( IAsyncResult ar )
		{
			Console.WriteLine( $"NetworkServer.OnAccept on { System.Threading.Thread.CurrentThread.ManagedThreadId }" );
			var client = listener.EndAcceptTcpClient( ar );
			accept.Set();
			Console.WriteLine( "ACCEPT" );
			var callback = ar.AsyncState as Action<NetworkClient>;
			callback( new NetworkClient( client ) );
		}
		#endregion
	}
}
