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
public class Game : PlayerMessage
{
        [XmlArrayItemAttribute(IsNullable=false)]
        public IList<Player> Players { get; set; }
        [XmlElement("Board")]
        public GameBoard Board { get; set; }
        [XmlElement("PlayerLocation")]
        public Location PlayerLocation { get; set; }
    
    public Game()
    {
        PlayerLocation = new Location();
        Board = new GameBoard();
        Players = new System.Collections.Generic.List<Player>();
    }
}
}
#pragma warning restore
