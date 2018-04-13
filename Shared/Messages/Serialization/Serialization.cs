using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Shared.Messages.Serialization
{
    public static class XMLSerialization
    {

        public static string Serialize<T>(
            T objectToSerialize,
            XmlSerializerNamespaces namespaces,
            XmlWriterSettings settings = default)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(xmlWriter, objectToSerialize, namespaces);
                }
                return stringWriter.ToString();
            }
        }


        private static Uri StringToUri(string address, UriKind uriKind = UriKind.Relative)
            => new Uri(address, uriKind);

        public static T XMLDeserializePath<T>(string xmlPath, string xsdData = null)
            => DeserializePath<T>(StringToUri(xmlPath), xsdData);


        public static T Deserialize<T>(string xmlDataAsString, string xsdData = null)
        {
            var serializer = new XmlSerializer(typeof(T));
            if (xsdData != null)
            {
                xmlDataAsString = ValidateSettings(xmlDataAsString, xsdData);
            }

            var resultingObject = serializer.Deserialize(new StringReader(xmlDataAsString));

            if (resultingObject is T)
            {
                return (T)resultingObject;
            }
            else
            {
                throw new Exception($"Deserialized object is not of type {default(T).GetType()}");
            }
        }

        public static T DeserializePath<T>(Uri xmlPath, string xsdData = null)
        {
            try
            {
                var xmlDataAsString = File.ReadAllText(xmlPath.ToString());
                return Deserialize<T>(xmlDataAsString, xsdData);
            }
            catch (Exception e)
            {
                if (File.Exists(xmlPath.ToString()))
                {
                    throw new IOException("Corrupt file", e);
                }
                else
                {
                    throw new IOException("Missing file", e);
                }

            }
        }

        private static string ValidateSettings(string xmlData, string xsdData)
        {
            var document = XDocument.Parse(xmlData);
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(XmlSchema.Read(new StringReader(xsdData), (o, e) =>
            {
                if (e.Exception != null)
                    throw e.Exception;
            }));

            document.Validate(schemaSet, (o, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                    throw e.Exception;
            });

            return xmlData;
        }
    }
}
