namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true)]
    public partial class GameInfo
    {

        private string gameNameField;

        private ulong redTeamPlayersField;

        private ulong blueTeamPlayersField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string gameName
        {
            get
            {
                return this.gameNameField;
            }
            set
            {
                this.gameNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong redTeamPlayers
        {
            get
            {
                return this.redTeamPlayersField;
            }
            set
            {
                this.redTeamPlayersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong blueTeamPlayers
        {
            get
            {
                return this.blueTeamPlayersField;
            }
            set
            {
                this.blueTeamPlayersField = value;
            }
        }
    }
}