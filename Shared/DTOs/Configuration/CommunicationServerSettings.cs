using System;
using System.Xml.Serialization;

namespace Shared.DTOs.Configuration
{
    [Serializable]
    [XmlType( AnonymousType = true, Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/" )]
    [XmlRoot( Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/", IsNullable = false )]
    public class CommunicationServerSettings : Configuration
    {
    }
}