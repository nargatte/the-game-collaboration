using Shared.Interfaces.Modules;

namespace Shared.Base.Modules
{
	public abstract class ModuleBase : StartableBase, IModule
	{
		#region IModule
		public virtual int Port { get; }
		#endregion
		#region ModuleBase
		public ModuleBase( int port ) => Port = port;
		#endregion
	}
}
