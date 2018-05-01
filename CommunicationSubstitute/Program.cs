using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using Shared.DTOs.Configuration;
using System;
using System.Threading;

namespace CommunicationSubstitute
{
	class Program
	{
		static void Main( string[] args )
		{
			var moduleCS = new CommunicationServerModule( 29170, new CommunicationServerSettings(), new CommunicationServerFactory() );
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
