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
        {
            if (_receivedMessage == null)
            {
                var data = new Byte[256];
                Int32 bytes;
                StringBuilder stringBuilder = new StringBuilder();
                do
                {
                    bytes = _stream.Read(data, 0, data.Length);
                    stringBuilder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                } while (data[bytes] != 23);

                _receivedMessage = stringBuilder.ToString();
            }

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
