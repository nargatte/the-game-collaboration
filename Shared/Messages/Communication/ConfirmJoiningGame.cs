namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class ConfirmJoiningGame : PlayerMessage
    {

        private Player playerDefinitionField;

        private ulong gameIdField;

        private string privateGuidField;

        /// <remarks/>
        public Player PlayerDefinition
        {
            get
            {
                return this.playerDefinitionField;
            }
            set
            {
                this.playerDefinitionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId
        {
            get
            {
                return this.gameIdField;
            }
            set
            {
                this.gameIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string privateGuid
        {
            get
            {
                return this.privateGuidField;
            }
            set
            {
                this.privateGuidField = value;
            }
        }
    }
}