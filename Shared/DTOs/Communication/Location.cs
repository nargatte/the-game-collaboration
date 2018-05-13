using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
    [XmlInclude( typeof( Field ) )]
    [XmlInclude( typeof( GoalField ) )]
    [XmlInclude( typeof( TaskField ) )]
    [Serializable]
    [XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
    public class Location
    {
		[XmlAttribute( "x" )]
		public uint X { get; set; }
		[XmlAttribute( "y" )]
		public uint Y { get; set; }
	}
}