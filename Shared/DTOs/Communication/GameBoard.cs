using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Communication
{
	[Serializable]
    [XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/" )]
    public class GameBoard
    {
		[XmlAttribute( "width" )]
		public uint Width { get; set; }
		[XmlAttribute( "tasksHeight" )]
		public uint TasksHeight { get; set; }
		[XmlAttribute( "goalsHeight" )]
		public uint GoalsHeight { get; set; }
	}
}