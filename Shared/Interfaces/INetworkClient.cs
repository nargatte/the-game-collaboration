using System;
using System.Text;
using Shared.Components.Serialization;

namespace Shared.Interfaces
{
    public interface INetworkClient
    {

        void Connect(string address, Int32 port);


        void Send<T>(T message);


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
        bool TryReceive<T>(out T message)
            where T : class;


        /// <summary>
        /// Make buffor empty
        /// </summary>
        void Discard();

    }
}