using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
	public abstract class BetweenPlayersMessage : PlayerMessage
    {
		[XmlAttribute( "senderPlayerId" )]
		public ulong SenderPlayerId { get; set; }
	}
}