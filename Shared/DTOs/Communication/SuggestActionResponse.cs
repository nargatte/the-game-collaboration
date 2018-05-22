// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.4.0.7
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace Shared.DTOs.Communication
{
using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.Collections.Generic;

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlTypeAttribute(AnonymousType=true, Namespace="https://se2.mini.pw.edu.pl/17-results/")]
[XmlRootAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/", IsNullable=false)]
public class SuggestActionResponse : BetweenPlayersMessage
{
        [XmlArrayItemAttribute(IsNullable=false)]
        public IList<TaskField> TaskFields { get; set; }
        [XmlArrayItemAttribute(IsNullable=false)]
        public IList<GoalField> GoalFields { get; set; }
    /// <summary>
    /// Signature of the player for confirming identity on GameMaster during messages exchange
    /// </summary>
        [XmlAttribute(AttributeName="playerGuid")]
        [RegularExpressionAttribute("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}")]
        public string PlayerGuid { get; set; }
        [XmlAttribute(AttributeName="gameId")]
        public ulong GameId { get; set; }
    
    public SuggestActionResponse()
    {
        GoalFields = new System.Collections.Generic.List<GoalField>();
        TaskFields = new System.Collections.Generic.List<TaskField>();
    }
}
}
#pragma warning restore
