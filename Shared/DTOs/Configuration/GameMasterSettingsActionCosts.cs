using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
    [Serializable]
    [XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    public class GameMasterSettingsActionCosts
    {
		public uint MoveDelay { get; set; } = 100u;
		public uint DiscoverDelay { get; set; } = 450u;
		public uint TestDelay { get; set; } = 500u;
		public uint PickUpDelay { get; set; } = 100u;
		public uint PlacingDelay { get; set; } = 100u;
		public uint KnowledgeExchangeDelay { get; set; } = 1200u;
	}
}