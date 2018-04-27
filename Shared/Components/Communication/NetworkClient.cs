using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components.Communication
{
    public class NetworkClient : IDisposable
    {
        public void Connect(string address, Int32 port)
        {
            TcpClient client = new TcpClient("localhost", port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            data = data.Concat(new Byte[] { 23 }).ToArray();

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);

            // Close everything.
            stream.Close();
            client.Close();
        }

        public void Send<T>(T message)
        {
            throw new NotImplementedException();
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

            throw new NotImplementedException();
        }

        /// <summary>
        /// Make buffor empty
        /// </summary>
        public void Discard()
        {

        }
        public void Dispose()
        {
            
        }
    }
}
