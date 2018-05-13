using Shared.Enums;
using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public class JoinGame
    {
		[XmlAttribute( "gameName" )]
		public string GameName { get; set; }
		[XmlAttribute( "preferredTeam" )]
		public TeamColour PreferredTeam { get; set; }
		[XmlAttribute( "preferredRole" )]
		public PlayerType PreferredRole { get; set; }
		[XmlAttribute( "playerId" )]
		public ulong PlayerId { get; set; }
		[XmlAttribute( "playerIdSpecified" )]
		public bool PlayerIdSpecified { get; set; }
	}
}