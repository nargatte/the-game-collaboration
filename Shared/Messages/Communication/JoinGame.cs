namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class JoinGame
    {

        private string gameNameField;

        private TeamColour preferredTeamField;

        private PlayerType preferredRoleField;

        private ulong playerIdField;

        private bool playerIdFieldSpecified;

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
        public TeamColour preferredTeam
        {
            get
            {
                return this.preferredTeamField;
            }
            set
            {
                this.preferredTeamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public PlayerType preferredRole
        {
            get
            {
                return this.preferredRoleField;
            }
            set
            {
                this.preferredRoleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong playerId
        {
            get
            {
                return this.playerIdField;
            }
            set
            {
                this.playerIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool playerIdSpecified
        {
            get
            {
                return this.playerIdFieldSpecified;
            }
            set
            {
                this.playerIdFieldSpecified = value;
            }
        }
    }
}