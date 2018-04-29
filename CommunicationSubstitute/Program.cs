using CommunicationServerCore.Components.Modules;
using Shared.Components.Communication;
using Shared.Components.Serialization;
using Shared.DTOs.Configuration;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommunicationSubstitute
{
	class Program
	{
		static void Main( string[] args )
		{
			var o = new CommunicationServerSettings();
			var moduleCS = new CommunicationServerModule( 29170, o );
			moduleCS.Exit += ( s, e ) =>
			{
				if( e.IsSuccess )
					Console.WriteLine( "Communication server module ended successfully." );
				else
					Console.WriteLine( $"Communication server module ended with exception: { e.Exception }." );
			};
			var threadCS = new Thread( moduleCS.Start );
			threadCS.Start();
			threadCS.Join();
		}
	}
}
