using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
	[XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
	[XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true )]
	public class TaskField : Field
    {
		[XmlAttribute( "distanceToPiece" )]
		public int DistanceToPiece { get; set; }
		[XmlAttribute( "pieceId" )]
		public ulong PieceId { get; set; }
		[XmlIgnore]
		public bool PieceIdSpecified { get; set; }
	}
}