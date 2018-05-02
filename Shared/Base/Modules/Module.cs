using Shared.Interfaces.Modules;

namespace Shared.Base.Modules
{
	public abstract class Module : StartableBase, IModule
	{
		#region IModule
		public virtual int Port { get; }
		#endregion
		#region Module
		public Module( int port ) => Port = port;
		#endregion
	}
}
