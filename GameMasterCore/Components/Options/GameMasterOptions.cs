using GameMasterCore.Interfaces.Options;
using Shared.Base.Options;
using Shared.Components.Options;
using Shared.DTO.Configuration;
using System.Collections.Generic;

namespace GameMasterCore.Components.Options
{
	public class GameMasterOptions : OptionsBase, IGameMasterOptions
	{
		#region IGameMasterOptions
		public virtual string Address { get; }
		public virtual GameMasterSettings Conf { get; }
		#endregion
		#region GameMasterOptions
		public GameMasterOptions( string[] args ) : this( CommandLineOptions.GetDictonary( args ) )
		{
		}
		protected GameMasterOptions( IDictionary<string, string> dictionary ) : base( dictionary )
		{
			Address = dictionary[ "address" ];
			Conf = CommandLineOptions.GetConfigFile<GameMasterSettings>( dictionary[ "conf" ] );
		}
		#endregion
	}
}
