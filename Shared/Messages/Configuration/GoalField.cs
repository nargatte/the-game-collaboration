using Shared.Enums;

namespace Shared.Messages.Configuration
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = true)]
    public partial class GoalField : Field
    {

        private GoalFieldType typeField;

        private TeamColour teamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public GoalFieldType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public TeamColour team
        {
            get
            {
                return this.teamField;
            }
            set
            {
                this.teamField = value;
            }
        }
    }
}