using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[XmlInclude( typeof( BetweenPlayersMessage ) )]
	[Serializable]
    [XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
    public class PlayerMessage
    {
		[XmlAttribute( "playerId" )]
		public ulong PlayerId { get; set; }
	}
}