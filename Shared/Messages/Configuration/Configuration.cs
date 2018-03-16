namespace Shared.Messages.Configuration
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = true)]
    public partial class Configuration
    {

        private uint keepAliveIntervalField;

        public Configuration()
        {
            this.keepAliveIntervalField = ((uint)(30000));
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "30000")]
        public uint KeepAliveInterval
        {
            get
            {
                return this.keepAliveIntervalField;
            }
            set
            {
                this.keepAliveIntervalField = value;
            }
        }
    }
}
