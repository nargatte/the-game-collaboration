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
[XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
[XmlRootAttribute("GameInfo")]
public class GameInfo
{
        [XmlAttribute(AttributeName="gameName")]
        [RegularExpressionAttribute("[A-Za-z][a-z0-9]*( [a-z0-9]+)*")]
        public string GameName { get; set; }
        [XmlAttribute(AttributeName="redTeamPlayers")]
        public ulong RedTeamPlayers { get; set; }
        [XmlAttribute(AttributeName="blueTeamPlayers")]
        public ulong BlueTeamPlayers { get; set; }
}
}
#pragma warning restore
