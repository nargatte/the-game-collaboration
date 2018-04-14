using System;

namespace Shared.Sockets
{
    public interface IDataAccess
    {
        void DataSend(object data);

        event EventHandler<object> DataReceive;
    }
}