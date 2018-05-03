﻿using CommunicationServerCore.Base.Modules;
using CommunicationServerCore.Interfaces.Factories;
using Shared.DTOs.Configuration;
using System;

namespace CommunicationServerCore.Components.Modules
{
	public class CommunicationServerModule : CommunicationServerModuleBase
	{
		#region CommunicationServerModuleBase
		public override void Start()
		{
			try
			{
				CommunicationServer.Finish += ( s, e ) =>
				{
					if( e.IsSuccess )
						Console.WriteLine( "Communication server finished successfully." );
					else
						Console.WriteLine( $"Communication server finished with exception: { e.Exception }." );
				};
				CommunicationServer.Start();
				OnFinish();
			}
			catch( Exception e )
			{
				OnFinish( e );
			}
		}
		#endregion
		#region CommunicationServerModule
		public CommunicationServerModule( int port, CommunicationServerSettings configuration, ICommunicationServerFactory factory ) : base( port, configuration, factory )
		{
		}
		#endregion
	}
}
