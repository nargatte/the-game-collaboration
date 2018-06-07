using CommunicationServerCore.Interfaces.Options;
using Shared.Base.Options;
using Shared.Components.Options;
using Shared.DTOs.Configuration;
using System.Collections.Generic;

namespace CommunicationServerCore.Components.Options
{
	public class CommunicationServerOptions : OptionsBase, ICommunicationServerOptions
	{
		#region ICommunicationServerOptions
		public virtual CommunicationServerSettings Conf { get; }
		#endregion
		#region CommunicationServerOptions
		public CommunicationServerOptions( string[] args ) : this( CommandLineOptions.GetDictonary( args ) )
		{
		}
		protected CommunicationServerOptions( IDictionary<string, string> dictionary ) : base( dictionary ) => Conf = CommandLineOptions.GetConfigFile<CommunicationServerSettings>( dictionary[ "conf" ] );
		#endregion
	}
}
