namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BetweenPlayersMessage))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true)]
    public partial class PlayerMessage
    {

        private ulong playerIdField;

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
    }
}