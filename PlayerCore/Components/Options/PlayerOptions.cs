using PlayerCore.Interfaces.Options;
using Shared.Base.Options;
using Shared.Components.Options;
using Shared.DTO.Configuration;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace PlayerCore.Components.Options
{
	public class PlayerOptions : OptionsBase, IPlayerOptions
	{
		#region IPlayerOptions
		public virtual string Address { get; }
		public virtual PlayerSettings Conf { get; }
		public virtual string Game { get; }
		public virtual TeamColour Team { get; }
		public virtual PlayerRole Role { get; }
		#endregion
		#region PlayerOptions
		public PlayerOptions( string[] args ) : this( CommandLineOptions.GetDictonary( args ) )
		{
		}
		protected PlayerOptions( IDictionary<string, string> dictionary ) : base( dictionary )
		{
			Address = dictionary[ "address" ];
			Conf = CommandLineOptions.GetConfigFile<PlayerSettings>( dictionary[ "conf" ] );
			Game = dictionary[ "game" ];
			switch( dictionary[ "team" ] )
			{
			case "red":
				Team = TeamColour.Red;
				break;
			case "blue":
				Team = TeamColour.Blue;
				break;
			default:
				throw new ArgumentOutOfRangeException( "team" );
			}
			switch( dictionary[ "role" ] )
			{
			case "leader":
				Role = PlayerRole.Leader;
				break;
			case "member":
				Role = PlayerRole.Member;
				break;
			default:
				throw new ArgumentOutOfRangeException( "role" );
			}
		}
		#endregion
	}
}
