using CommunicationServerCore.Base.Servers;
using Shared.Components.Extensions;
using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServerCore.Components.Servers
{
	public class CommunicationServer : CommunicationServerBase
	{
		#region CommunicationServerBase
		public override async Task RunAsync( CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			var tasks = new List<Task>();
			try
			{
				using( var server = Factory.MakeNetworkServer( Ip, Port ) )
				{
					while( true )
					{
						var client = await server.AcceptAsync( cancellationToken ).ConfigureAwait( false );
						tasks.Add( Task.Run( async () => await OnAcceptAsync( client, cancellationToken ).ConfigureAwait( false ) ) );
					}
				}
			}
			finally
			{
				try
				{
					await Task.WhenAll( tasks );
				}
				catch( Exception )
				{
				}
				foreach( var task in tasks )
				{
					if( task.IsCanceled )
						Console.WriteLine( $"server task canceled" );
					else if( task.IsFaulted )
						Console.WriteLine( $"server task faulted with { task.Exception }" );
					else
						Console.WriteLine( "server task completed" );
				}
			}
		}
		#endregion
		#region CommunicationServer
		public CommunicationServer( string ip, int port, uint keepAliveInterval, INetworkFactory factory ) : base( ip, port, keepAliveInterval, factory )
		{
		}
		protected Task OnAcceptAsync( INetworkClient client, CancellationToken cancellationToken )
		{
			cancellationToken.ThrowIfCancellationRequested();
			using( client )
			{

			}
			return Task.CompletedTask;
		}
		#endregion
	}
}
