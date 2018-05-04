using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using PlayerCore.Components.Modules;
using Shared.DTOs.Configuration;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationSubstitute
{
	class Program
	{
		public static async Task Main( string[] args )
		{
			try
			{
				int port = 65535;
				var cts = new CancellationTokenSource( 10000 );
				var cs = new CommunicationServerModule( port, new CommunicationServerSettings(), new CommunicationServerFactory() );
				var p1 = new PlayerModule( port, new PlayerSettings() );
				var tasks = new List<Task>
				{
					Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) )
				};
				try
				{
					await Task.WhenAll( tasks );
				}
				catch( Exception )
				{
					foreach( var task in tasks )
					{
						if( task.IsCanceled )
							Console.WriteLine( $"task canceled" );
						else if( task.IsFaulted )
							Console.WriteLine( $"task faulted with { task.Exception }" );
						else
							Console.WriteLine( "task completed" );
					}
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( e );
			}
		}
	}
}
