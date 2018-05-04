using Shared.Interfaces.Modules;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Base.Modules
{
	public abstract class ModuleBase : IModule
	{
		#region IModule
		public abstract Task RunAsync( CancellationToken ct );
		public virtual int Port { get; }
		#endregion
		#region ModuleBase
		public ModuleBase( int port ) => Port = port;
		#endregion
	}
}
