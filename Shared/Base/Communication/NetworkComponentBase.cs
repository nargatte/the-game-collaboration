using Shared.Interfaces.Communication;
using Shared.Interfaces.Factories;
using System;

namespace Shared.Base.Communication
{
	public abstract class NetworkComponentBase : INetworkComponent
	{
		#region INetworkComponent
		public abstract void Dispose();
		public virtual INetworkFactory Factory { get; }
		#endregion
		#region NetworkComponentBase
		public NetworkComponentBase( INetworkFactory factory ) => Factory = factory is null ? throw new ArgumentNullException( nameof( factory ) ) : factory;
		#endregion
	}
}
