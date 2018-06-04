using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Shared.Components.ArgumentOptions
{
    public class GameMasterOptions : CommunicationServerOptions
    {
        public GameMasterOptions(Dictionary<string, string> dictionary) : base(dictionary)
        {
            Address = dictionary["address"];
        }

        public string Address { get; }
    }
}