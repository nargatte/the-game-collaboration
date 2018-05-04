using CommunicationServerCore.Components.Factories;
using CommunicationServerCore.Components.Modules;
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
				var cts = new CancellationTokenSource();
				var cs = new CommunicationServerModule( 65535, new CommunicationServerSettings(), new CommunicationServerFactory() );
				Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: starting all tasks" );
				var tasks = new List<Task>
				{
					Task.Run( () => cs.RunAsync( cts.Token ) )
				};
				try
				{
					Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: completing all tasks" );
					await Task.WhenAll( tasks );
				}
				catch( Exception )
				{
				}
				Console.WriteLine( $"{ Thread.CurrentThread.ManagedThreadId }: logging all tasks:" );
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
