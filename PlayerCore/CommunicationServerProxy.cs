using PlayerCore.Interfaces;
using System;
using System.Net.Sockets;
using System.Linq;
using Shared.Components.Communication;

namespace PlayerCore
{
    class CommunicationServerProxy : NetworkClient
    {
        public IPlayer Player { get; set; }

        public CommunicationServerProxy()
        {

        }

        
    }
}
