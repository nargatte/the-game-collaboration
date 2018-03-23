using System;
using System.Xml.Serialization;

namespace Shared.Enums
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false )]
	public enum PieceType
    {
		[XmlEnum( "unknown" )]
		Unknown,
		[XmlEnum( "sham" )]
		Sham,
		[XmlEnum( "normal" )]
		Normal
    }
}
