using System;
using System.Xml.Serialization;

namespace Shared.Enums
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public enum GoalFieldType
    {
		[XmlIgnore]
		Unknown,
		[XmlEnum( "goal" )]
		Goal,
		[XmlEnum( "non-goal" )]
		NonGoal
    }
}
