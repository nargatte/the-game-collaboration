using Shared.Base.Communication;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Components.Communication
{
	public class NetworkClient : NetworkClientBase
    {
		#region NetworkClientBase
		public override void Dispose()
		{
			stream.Dispose();
			base.Dispose();
		}
		public override async Task SendAsync( string message, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			byte[] data = ConstHelper.Encoding.GetBytes( message + ConstHelper.EndOfMessage );
			await stream.WriteAsync( data, 0, data.Length, cancellationToken ).ConfigureAwait( false );
		}
		public override async Task<string> ReceiveAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			byte[] data = new byte[ 256 ];
			while( !builder.ToString().Contains( ConstHelper.EndOfMessage ) )
			{
				int bytes = await stream.ReadAsync( data, 0, data.Length, cancellationToken ).ConfigureAwait( false );
				builder.Append( ConstHelper.Encoding.GetString( data, 0, bytes ) );
			}
			string buffer = builder.ToString();
			int pos = buffer.IndexOf( ConstHelper.EndOfMessage );
			builder.Remove( 0, pos + ConstHelper.EndOfMessage.Length );
			return buffer.Substring( 0, pos );
		}
		#endregion
		#region NetworkClient
		private NetworkStream stream;
		private StringBuilder builder = new StringBuilder();
		public NetworkClient( TcpClient client ) : base( client ) => stream = Client.GetStream();
		#endregion
    }
}
