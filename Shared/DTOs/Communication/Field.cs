using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[XmlInclude( typeof( GoalField ) )]
	[XmlInclude( typeof( TaskField ) )]
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
	public abstract class Field : Location
    {
		[XmlAttribute( "timestamp" )]
		public System.DateTime Timestamp { get; set; }
		[XmlAttribute( "playerId" )]
		public ulong PlayerId { get; set; }
		[XmlIgnore]
		public bool PlayerIdSpecified { get; set; }
	}
}