using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Shared.Components.Serialization;

namespace Shared.Components.Communication
{
    public class NetworkClient : IDisposable
    {
		#region INetworkClient
		#endregion
		#region NetworkClient
		private TcpClient client;
		public NetworkClient( TcpClient aClient ) => client = aClient;
		#endregion


		//

		private TcpClient _client;
        private NetworkStream _stream;
        private string _receivedMessage = null; 

        public void Connect(string address, Int32 port)
        {
            _client = new TcpClient(address, port);

            _stream = _client.GetStream();
        }

        public void Send<T>(T message)
        {
            string desMessage = Serializer.Serialize(message);

            Byte[] data = Encoding.ASCII.GetBytes(desMessage);
            data = data.Concat(new Byte[] { 23 }).ToArray();

            _stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// If buffor empty -> block and try deserialize
        /// otherwise -> only try deserialize
        /// 
        /// If deserialise end success -> remove bufor
        /// otherwise -> don't remove
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool TryReceive<T>(out T message)
            where T: class
        {
            if (_receivedMessage == null)
            {
                var data = new Byte[256];
                Int32 bytes;
                StringBuilder stringBuilder = new StringBuilder();
                do
                {
                    bytes = _stream.Read(data, 0, data.Length);

                    if (bytes == 1 && data[0] == 23)
                    {
                        _stream.Write(new byte[] {23}, 0, 1);
                        continue;
                    }

                    stringBuilder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                } while (data[bytes - 3] != 23 && data[bytes - 1] != 23); //first case is for debugging with socet test v3, the last bytes is end of line
                    
                _receivedMessage = stringBuilder.ToString();

                if(data[bytes - 3] == 23)
                    _receivedMessage = _receivedMessage.Substring(0, _receivedMessage.Length - 3);

                if (data[bytes - 1] == 23)
                    _receivedMessage = _receivedMessage.Substring(0, _receivedMessage.Length - 2);
            }

            message = Deserializer.Deserialize<T>(_receivedMessage);

            if (message == null)
                return false;

            Discard();
            return true;
        }

        /// <summary>
        /// Make buffor empty
        /// </summary>
        public void Discard()
        {
            _receivedMessage = null;
        }

        public void Dispose()
        {
            _stream.Close();
            _client.Close();
        }
    }
}
