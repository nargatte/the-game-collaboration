using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
    [Serializable]
    [XmlType( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = true )]
    public class Configuration
    {
		[XmlAttribute]
		[DefaultValue( typeof( uint ), "30000" )]
		public uint KeepAliveInterval { get; set; }
		public Configuration() => KeepAliveInterval = 30000u;
	}
}
