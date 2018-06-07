using Shared.Interfaces.Options;
using System.Collections.Generic;

namespace Shared.Base.Options
{
	public abstract class OptionsBase : IOptions
	{
		#region IOptions
		public virtual int Port { get; }
		#endregion
		#region OptionsBase
		protected OptionsBase( IDictionary<string, string> dictionary ) => Port = int.Parse( dictionary[ "port" ] );
		#endregion
	}
}
