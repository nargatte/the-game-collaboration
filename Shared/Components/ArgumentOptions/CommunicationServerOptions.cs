using System;
using System.Collections.Generic;

namespace Shared.Components.ArgumentOptions
{
    public class CommunicationServerOptions
    {
        public Int32 Port { get; }
        public string Conf { get; }

        public CommunicationServerOptions(Dictionary<String, String> dictionary)
        {
            Port = Int32.Parse(dictionary["port"]);
            Conf = dictionary["conf"];
        }
    }
}