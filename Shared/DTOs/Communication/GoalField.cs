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
	using Shared.Enums;

	[System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
[Serializable]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[XmlTypeAttribute(Namespace="https://se2.mini.pw.edu.pl/17-results/")]
[XmlRootAttribute("GoalField")]
public class GoalField : Field
{
        [XmlAttribute(AttributeName="type")]
        public GoalFieldType Type { get; set; }
        [XmlAttribute(AttributeName="team")]
        public TeamColour Team { get; set; }
}
}
#pragma warning restore
