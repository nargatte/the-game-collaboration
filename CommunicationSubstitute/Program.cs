using CommunicationServerCore.Components.Modules;
using System.Threading;

namespace CommunicationSubstitute
{
    class Program
    {
        static void Main( string[] args )
        {
			var moduleCS = new CommunicationServerModule();
			var threadCS = new Thread( moduleCS.Start );
			threadCS.Start();
			threadCS.Join();
		}
    }
}
