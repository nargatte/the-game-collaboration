namespace Shared.Messages.Configuration
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public partial class GameMasterSettingsActionCosts
    {

        private uint moveDelayField;

        private uint discoverDelayField;

        private uint testDelayField;

        private uint pickUpDelayField;

        private uint placingDelayField;

        private uint knowledgeExchangeDelayField;

        public GameMasterSettingsActionCosts()
        {
            this.moveDelayField = ((uint)(100));
            this.discoverDelayField = ((uint)(450));
            this.testDelayField = ((uint)(500));
            this.pickUpDelayField = ((uint)(100));
            this.placingDelayField = ((uint)(100));
            this.knowledgeExchangeDelayField = ((uint)(1200));
        }

        /// <remarks/>
        public uint MoveDelay
        {
            get
            {
                return this.moveDelayField;
            }
            set
            {
                this.moveDelayField = value;
            }
        }

        /// <remarks/>
        public uint DiscoverDelay
        {
            get
            {
                return this.discoverDelayField;
            }
            set
            {
                this.discoverDelayField = value;
            }
        }

        /// <remarks/>
        public uint TestDelay
        {
            get
            {
                return this.testDelayField;
            }
            set
            {
                this.testDelayField = value;
            }
        }

        /// <remarks/>
        public uint PickUpDelay
        {
            get
            {
                return this.pickUpDelayField;
            }
            set
            {
                this.pickUpDelayField = value;
            }
        }

        /// <remarks/>
        public uint PlacingDelay
        {
            get
            {
                return this.placingDelayField;
            }
            set
            {
                this.placingDelayField = value;
            }
        }

        /// <remarks/>
        public uint KnowledgeExchangeDelay
        {
            get
            {
                return this.knowledgeExchangeDelayField;
            }
            set
            {
                this.knowledgeExchangeDelayField = value;
            }
        }
    }
}