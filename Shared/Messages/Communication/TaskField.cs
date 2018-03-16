namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = true)]
    public partial class TaskField : Field
    {

        private int distanceToPieceField;

        private ulong pieceIdField;

        private bool pieceIdFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int distanceToPiece
        {
            get
            {
                return this.distanceToPieceField;
            }
            set
            {
                this.distanceToPieceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong pieceId
        {
            get
            {
                return this.pieceIdField;
            }
            set
            {
                this.pieceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool pieceIdSpecified
        {
            get
            {
                return this.pieceIdFieldSpecified;
            }
            set
            {
                this.pieceIdFieldSpecified = value;
            }
        }
    }
}