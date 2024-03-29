// ------------------------------------------------------------------------------
//  <auto-generated>
//    Generated by Xsd2Code++. Version 4.4.0.7
//  </auto-generated>
// ------------------------------------------------------------------------------
#pragma warning disable
namespace Shared.DTO.Communication
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
[XmlRootAttribute("Piece")]
public class Piece
{
    private System.Nullable<ulong> _playerId;
        [XmlAttribute(AttributeName="id")]
        public ulong Id { get; set; }
        [XmlAttribute(AttributeName="type")]
        public PieceType Type { get; set; }
        [XmlAttribute(AttributeName="timestamp")]
        public System.DateTime Timestamp { get; set; }
    
    [XmlAttribute(AttributeName="playerId")]
    public ulong PlayerId
    {
        get
        {
            if (_playerId.HasValue)
            {
                return _playerId.Value;
            }
            else
            {
                return default(ulong);
            }
        }
        set
        {
            _playerId = value;
        }
    }
    
    [XmlIgnore]
    public bool PlayerIdSpecified
    {
        get
        {
            return _playerId.HasValue;
        }
        set
        {
            if (value==false)
            {
                _playerId = null;
            }
        }
    }
}
}
#pragma warning restore
