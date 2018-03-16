namespace Shared.Messages.Configuration
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = false)]
    public partial class GameMasterSettings : Configuration
    {

        private GameMasterSettingsGameDefinition gameDefinitionField;

        private GameMasterSettingsActionCosts actionCostsField;

        private uint retryRegisterGameIntervalField;

        public GameMasterSettings()
        {
            this.retryRegisterGameIntervalField = ((uint)(5000));
        }

        /// <remarks/>
        public GameMasterSettingsGameDefinition GameDefinition
        {
            get
            {
                return this.gameDefinitionField;
            }
            set
            {
                this.gameDefinitionField = value;
            }
        }

        /// <remarks/>
        public GameMasterSettingsActionCosts ActionCosts
        {
            get
            {
                return this.actionCostsField;
            }
            set
            {
                this.actionCostsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "5000")]
        public uint RetryRegisterGameInterval
        {
            get
            {
                return this.retryRegisterGameIntervalField;
            }
            set
            {
                this.retryRegisterGameIntervalField = value;
            }
        }
    }
}