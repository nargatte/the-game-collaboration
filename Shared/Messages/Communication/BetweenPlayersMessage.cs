namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true)]
    public abstract partial class BetweenPlayersMessage : PlayerMessage
    {

        private ulong senderPlayerIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong senderPlayerId
        {
            get
            {
                return this.senderPlayerIdField;
            }
            set
            {
                this.senderPlayerIdField = value;
            }
        }
    }
}