using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public class RegisteredGames
    {
		[XmlElement( "GameInfo" )]
		public GameInfo[] GameInfo { get; set; }
	}
}