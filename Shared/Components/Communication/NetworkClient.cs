using Shared.Base.Communication;
using Shared.Interfaces.Factories;
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
			stream.Close();
			base.Dispose();
		}
		public override async Task SendAsync( string message, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			byte[] data = Encoding.ASCII.GetBytes( message );
			await stream.WriteAsync( data, 0, data.Length, cancellationToken );
		}
		public override async Task<string> ReceiveAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			byte[] data = new byte[ 256 ];
			while( !builder.ToString().Contains( ConstHelper.EndOfMessage ) )
			{
				int bytes = await stream.ReadAsync( data, 0, data.Length, cancellationToken );
				builder.Append( Encoding.ASCII.GetString( data, 0, bytes ) );
			}
			string buffer = builder.ToString();
			int pos = buffer.IndexOf( ConstHelper.EndOfMessage );
			builder.Remove( 0, pos + ConstHelper.EndOfMessage.Length );
			return buffer.Substring( 0, pos + ConstHelper.EndOfMessage.Length );
		}
		#endregion
		#region NetworkClient
		private NetworkStream stream;
		private StringBuilder builder = new StringBuilder();
		public NetworkClient( TcpClient client, INetworkFactory factory ) : base( client, factory ) => stream = Client.GetStream();
		#endregion
    }
}
