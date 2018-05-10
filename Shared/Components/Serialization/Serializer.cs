using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Components.Serialization
{
	public static class Serializer
	{
		public static string Serialize<T>( T o ) where T : class
		{
			var type = typeof( T );
			var xmlWriterSettings = new XmlWriterSettings
			{
				Indent = true
			};
			var xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add( string.Empty, ( type.GetCustomAttributes( typeof( XmlTypeAttribute ), true ).FirstOrDefault() as XmlTypeAttribute )?.Namespace );
			var serializer = new XmlSerializer( type );
			using( var stringWriter = new StringWriterWithEncoding( ConstHelper.Encoding ) )
			{
				using( var xmlWriter = XmlWriter.Create( stringWriter, xmlWriterSettings ) )
				{
					serializer.Serialize( xmlWriter, o, xmlSerializerNamespaces );
					return stringWriter.ToString();
				}
			}
		}
		public static T Deserialize< T >( string s ) where T : class
		{
			var serializer = new XmlSerializer( typeof( T ) );
			using( var stringReader = new StringReader( s ) )
			{
				using( var xmlReader = XmlReader.Create( stringReader ) )
				{
					return serializer.CanDeserialize( xmlReader ) ? serializer.Deserialize( xmlReader ) as T : null;
				}
			}
		}
	}
}
