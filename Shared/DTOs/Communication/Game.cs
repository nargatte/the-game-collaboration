using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public class Game : PlayerMessage
    {
		[XmlArrayItem( IsNullable = false )]
		public Player[] Players { get; set; }
		public GameBoard Board { get; set; }
		public Location PlayerLocation { get; set; }
	}
}