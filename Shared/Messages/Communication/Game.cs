namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class Game : PlayerMessage
    {

        private Player[] playersField;

        private GameBoard boardField;

        private Location playerLocationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public Player[] Players
        {
            get
            {
                return this.playersField;
            }
            set
            {
                this.playersField = value;
            }
        }

        /// <remarks/>
        public GameBoard Board
        {
            get
            {
                return this.boardField;
            }
            set
            {
                this.boardField = value;
            }
        }

        /// <remarks/>
        public Location PlayerLocation
        {
            get
            {
                return this.playerLocationField;
            }
            set
            {
                this.playerLocationField = value;
            }
        }
    }
}