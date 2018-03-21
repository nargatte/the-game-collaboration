namespace Shared.Messages.Configuration
{


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = false)]
    public partial class PlayerSettings : Configuration
    {

        private uint retryJoinGameIntervalField;

        public PlayerSettings()
        {
            this.retryJoinGameIntervalField = ((uint)(5000));
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "5000")]
        public uint RetryJoinGameInterval
        {
            get
            {
                return this.retryJoinGameIntervalField;
            }
            set
            {
                this.retryJoinGameIntervalField = value;
            }
        }
    }
}