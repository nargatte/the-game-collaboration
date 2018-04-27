using CommunicationServerCore.Components.Modules;
using Shared.Components.Serialization;
using Shared.DTOs.Configuration;
using System.Threading;

namespace CommunicationSubstitute
{
    class Program
    {
        static void Main( string[] args )
        {
			var o = new CommunicationServerSettings() { KeepAliveInterval = 200 };
			var s = Serializer.Serialize( o );
			var moduleCS = new CommunicationServerModule( 2300, o );
			var threadCS = new Thread( moduleCS.Start );
			threadCS.Start();
			threadCS.Join();
		}
    }
}
