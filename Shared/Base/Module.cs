using System;
using Shared.Components.Events;
using Shared.Interfaces;

namespace Shared.Base
{
	public abstract class Module : IModule
	{
		#region IModule
		public abstract void Start();
		public virtual int Port { get; }
		public virtual event EventHandler<ExitArgs> Exit;
		#endregion
		#region Module
		public Module( int port ) => Port = port;
		protected void OnExit( Exception exception = null ) => EventHelper.OnEvent( this, Exit, new ExitArgs( exception ) );
		#endregion
	}
}
