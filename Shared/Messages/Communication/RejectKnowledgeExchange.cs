namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class RejectKnowledgeExchange : BetweenPlayersMessage
    {

        private bool permanentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool permanent
        {
            get
            {
                return this.permanentField;
            }
            set
            {
                this.permanentField = value;
            }
        }
    }
}