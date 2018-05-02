using Shared.Components.Events;
using Shared.Interfaces;
using System;

namespace Shared.Base
{
	public abstract class StartableBase : IStartable
	{
		#region IStartable
		public abstract void Start();
		public virtual event EventHandler<FinishArgs> Finish = delegate { };
		#endregion
		#region StartableBase
		protected void OnFinish( Exception exception = null ) => EventHelper.OnEvent( this, Finish, new FinishArgs( exception ) );
		#endregion
	}
}
