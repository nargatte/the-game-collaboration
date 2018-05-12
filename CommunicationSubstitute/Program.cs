using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
using GameMasterCore.Components.Factories;
using GameMasterCore.Components.Modules;
using PlayerCore.Components.Factories;
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
				string ip = "127.0.0.1";
				int timeout = 15000;
				int port = 65535;
				using( var cts = new CancellationTokenSource( timeout ) )
				{
					var cs = new CommunicationServerModule( ip, port, new CommunicationServerSettings(), new CommunicationServerFactory() );
					var gm1 = new GameMasterModule( ip, port, new GameMasterSettings(), new GameMasterFactory() );
					var p1 = new PlayerModule( ip, port, new PlayerSettings(), new PlayerFactory() );
					var p2 = new PlayerModule( ip, port, new PlayerSettings(), new PlayerFactory() );
					var tasks = new List<Task>
					{
						Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						//Task.Run( async () => await gm1.RunAsync( cts.Token ).ConfigureAwait( false ) ),
						Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) )//,
						//Task.Run( async () => await p2.RunAsync( cts.Token ).ConfigureAwait( false ) )
					};
					try
					{
						await Task.WhenAll( tasks );
					}
					catch( OperationCanceledException )
					{
					}
					finally
					{
						foreach( var task in tasks )
						{
							if( task.IsFaulted )
								Console.WriteLine( $"Module task faulted with { task.Exception.GetType().Name }." );
							else if( task.IsCanceled )
								Console.WriteLine( $"Module task canceled." );
							else
								Console.WriteLine( "Module task completed." );
						}
					}
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( e );//.GetType().Name );
			}
		}
	}
}
