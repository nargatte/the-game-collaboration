namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public partial class GameBoard
    {

        private uint widthField;

        private uint tasksHeightField;

        private uint goalsHeightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint tasksHeight
        {
            get
            {
                return this.tasksHeightField;
            }
            set
            {
                this.tasksHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint goalsHeight
        {
            get
            {
                return this.goalsHeightField;
            }
            set
            {
                this.goalsHeightField = value;
            }
        }
    }
}