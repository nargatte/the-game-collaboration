using Shared.Enums;
using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
	public class GoalField : Field
    {
		[XmlAttribute( "type" )]
		public GoalFieldType Type { get; set; }
		[XmlAttribute( "team" )]
		public TeamColour Team { get; set; }
	}
}