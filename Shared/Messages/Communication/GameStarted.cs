﻿namespace Shared.Messages.Communication
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xsd2", "1.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://se2.mini.pw.edu.pl/17-results/", IsNullable = false)]
    public partial class GameStarted
    {

        private ulong gameIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong gameId
        {
            get
            {
                return this.gameIdField;
            }
            set
            {
                this.gameIdField = value;
            }
        }
    }
}