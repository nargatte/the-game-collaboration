using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
    [XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
    public class GameInfo
    {
		[XmlAttribute( "gameName" )]
		public string GameName { get; set; }
		[XmlAttribute( "redTeamPlayers" )]
		public ulong RedTeamPlayers { get; set; }
		[XmlAttribute( "blueTeamPlayers" )]
		public ulong BlueTeamPlayers { get; set; }
	}
}