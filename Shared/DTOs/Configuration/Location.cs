using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
	[XmlInclude( typeof( Field ) )]
    [XmlInclude( typeof( GoalField ) )]
    [Serializable]
    [XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = true )]
    public class Location
    {
		[XmlAttribute( "x" )]
		public uint X { get; set; }
		[XmlAttribute( "y" )]
		public uint Y { get; set; }
	}
}