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
				var cts = new CancellationTokenSource();
				var cs = new CommunicationServerModule( port, new CommunicationServerSettings(), new CommunicationServerFactory() );
				var p1 = new PlayerModule( port, new PlayerSettings() );
				Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: starting all tasks" );
				var tasks = new List<Task>
				{
					Task.Run( async () => await cs.RunAsync( cts.Token ).ConfigureAwait( false ) ),
					Task.Run( async () => await p1.RunAsync( cts.Token ).ConfigureAwait( false ) )
				};
				try
				{
					Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: completing all tasks" );
					await Task.WhenAll( tasks );
				}
				catch( Exception )
				{
				}
				Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: logging all tasks" );
				foreach( var task in tasks )
				{
					if( task.IsFaulted )
						Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: task failed with { task.Exception }" );
					else if( task.IsCanceled )
						Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: task canceled" );
					else if( task.IsCompleted )
						Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: task completed" );
					else
						Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: ?" );
				}
			}
			catch( Exception e )
			{
				Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: { e }" );
			}
		}
	}
}
