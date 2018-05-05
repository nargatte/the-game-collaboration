using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
    [Serializable]
    [XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = false )]
    public class GameMasterSettings : Configuration
    {
		public GameMasterSettingsGameDefinition GameDefinition { get; set; }
		public GameMasterSettingsActionCosts ActionCosts { get; set; }
		[XmlAttribute]
		[DefaultValue( typeof( uint ), "5000" )]
		public uint RetryRegisterGameInterval { get; set; } = 5000u;
	}
}