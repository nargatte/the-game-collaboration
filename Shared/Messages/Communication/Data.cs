namespace Shared.Messages.Communication
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class Data : PlayerMessage
    {
        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(TaskFields)}: [\n{string.Join("\n", (object[]) (TaskFields ?? new TaskField[0]))}\n],\n" +
                   $" {nameof(GoalFields)}: [\n{string.Join("\n", (object[])(GoalFields ?? new GoalField[0]))}\n],\n" +
                   $" {nameof(Pieces)}: [\n{string.Join("\n", (object[])(Pieces ?? new Piece[0]))}\n],\n" +
                   $" {nameof(PlayerLocation)}: {PlayerLocation}, {nameof(gameFinished)}: {gameFinished}";
        }

        private TaskField[] taskFieldsField;

        private GoalField[] goalFieldsField;

        private Piece[] piecesField;

        private Location playerLocationField;

        private bool gameFinishedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public TaskField[] TaskFields
        {
            get
            {
                return this.taskFieldsField;
            }
            set
            {
                this.taskFieldsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public GoalField[] GoalFields
        {
            get
            {
                return this.goalFieldsField;
            }
            set
            {
                this.goalFieldsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public Piece[] Pieces
        {
            get
            {
                return this.piecesField;
            }
            set
            {
                this.piecesField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool gameFinished
        {
            get
            {
                return this.gameFinishedField;
            }
            set
            {
                this.gameFinishedField = value;
            }
        }
    }
}