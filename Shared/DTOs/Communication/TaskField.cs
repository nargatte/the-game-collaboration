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
[XmlRootAttribute("TaskField")]
public class TaskField : Field
{
    private System.Nullable<ulong> _pieceId;
        [XmlAttribute(AttributeName="distanceToPiece")]
        public int DistanceToPiece { get; set; }
    
    [XmlAttribute(AttributeName="pieceId")]
    public ulong PieceId
    {
        get
        {
            if (_pieceId.HasValue)
            {
                return _pieceId.Value;
            }
            else
            {
                return default(ulong);
            }
        }
        set
        {
            _pieceId = value;
        }
    }
    
    [XmlIgnore]
    public bool PieceIdSpecified
    {
        get
        {
            return _pieceId.HasValue;
        }
        set
        {
            if (value==false)
            {
                _pieceId = null;
            }
        }
    }
}
}
#pragma warning restore