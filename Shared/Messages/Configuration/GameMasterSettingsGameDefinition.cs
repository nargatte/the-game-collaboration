namespace Shared.Messages.Configuration
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public partial class GameMasterSettingsGameDefinition
    {

        private GoalField[] goalsField;

        private double shamProbabilityField;

        private uint placingNewPiecesFrequencyField;

        private uint initialNumberOfPiecesField;

        private uint boardWidthField;

        private uint taskAreaLengthField;

        private uint goalAreaLengthField;

        private uint numberOfPlayersPerTeamField;

        private string gameNameField;

        public GameMasterSettingsGameDefinition()
        {
            this.shamProbabilityField = 0.1D;
            this.placingNewPiecesFrequencyField = ((uint)(1000));
            this.initialNumberOfPiecesField = ((uint)(4));
            this.boardWidthField = ((uint)(5));
            this.taskAreaLengthField = ((uint)(7));
            this.goalAreaLengthField = ((uint)(3));
            this.numberOfPlayersPerTeamField = ((uint)(1));
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Goals")]
        public GoalField[] Goals
        {
            get
            {
                return this.goalsField;
            }
            set
            {
                this.goalsField = value;
            }
        }

        /// <remarks/>
        public double ShamProbability
        {
            get
            {
                return this.shamProbabilityField;
            }
            set
            {
                this.shamProbabilityField = value;
            }
        }

        /// <remarks/>
        public uint PlacingNewPiecesFrequency
        {
            get
            {
                return this.placingNewPiecesFrequencyField;
            }
            set
            {
                this.placingNewPiecesFrequencyField = value;
            }
        }

        /// <remarks/>
        public uint InitialNumberOfPieces
        {
            get
            {
                return this.initialNumberOfPiecesField;
            }
            set
            {
                this.initialNumberOfPiecesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public uint BoardWidth
        {
            get
            {
                return this.boardWidthField;
            }
            set
            {
                this.boardWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public uint TaskAreaLength
        {
            get
            {
                return this.taskAreaLengthField;
            }
            set
            {
                this.taskAreaLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public uint GoalAreaLength
        {
            get
            {
                return this.goalAreaLengthField;
            }
            set
            {
                this.goalAreaLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "nonNegativeInteger")]
        public uint NumberOfPlayersPerTeam
        {
            get
            {
                return this.numberOfPlayersPerTeamField;
            }
            set
            {
                this.numberOfPlayersPerTeamField = value;
            }
        }

        /// <remarks/>
        public string GameName
        {
            get
            {
                return this.gameNameField;
            }
            set
            {
                this.gameNameField = value;
            }
        }
    }
}