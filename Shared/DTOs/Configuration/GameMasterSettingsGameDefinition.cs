using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
    [Serializable]
    [XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    public class GameMasterSettingsGameDefinition
    {
		[XmlElement( "Goals" )]
		public GoalField[] Goals { get; set; }
		public double ShamProbability { get; set; } = 0D;
		public uint PlacingNewPiecesFrequency { get; set; } = 1000u;
		public uint InitialNumberOfPieces { get; set; } = 10u;
		public uint BoardWidth { get; set; } = 6u;
		public uint TaskAreaLength { get; set; } = 6u;
		public uint GoalAreaLength { get; set; } = 2u;
		public uint NumberOfPlayersPerTeam { get; set; } = 5u;
		public string GameName { get; set; }
	}
}