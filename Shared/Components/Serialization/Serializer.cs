using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Components.Serialization
{
	public static class Serializer
	{
		public static string Serialize< T >( T o ) => Serialize( o, typeof( T ) );
		public static string Serialize( object o, Type type )
		{
			var xmlWriterSettings = new XmlWriterSettings
			{
				Indent = true
			};
			var xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add( string.Empty, ( type.GetCustomAttributes( typeof( XmlTypeAttribute ), true ).FirstOrDefault() as XmlTypeAttribute )?.Namespace );
			var serializer = new XmlSerializer( type );
			using( var stringWriter = new StringWriterWithEncoding( Encoding.UTF8 ) )
			{
				using( var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings ) )
				{
					serializer.Serialize( xmlWriter, o, xmlSerializerNamespaces );
					return stringWriter.ToString();
				}
			}
		}
	}
}
