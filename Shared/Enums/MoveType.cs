using System;
using System.Xml.Serialization;

namespace Shared.Enums
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public enum MoveType
	{
		[XmlEnum( "up" )]
		Up,
		[XmlEnum( "down" )]
		Down,
		[XmlEnum( "left" )]
		Left,
		[XmlEnum( "right" )]
		Right,
	}
}
