using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Shared.Messages.Deserialization
{
    static class Deserialization
    {
        private static Uri StringToUri(string address, UriKind uriKind = UriKind.Relative)
            => new Uri(address, uriKind);

        public static T XMLDeserialize<T>(string xmlPath, string xsdData = null)
            => XMLDeserializeAsync<T>(StringToUri(xmlPath), xsdData);

        public static T XMLDeserializeAsync<T>(Uri xmlPath, string xsdData = null)
        {
            var serializer = new XmlSerializer(typeof(T));

            try
            {
                var xmlData = File.ReadAllText(xmlPath.ToString());

                if (xsdData != null)
                {
                    xmlData = ValidateSettings(xmlData, xsdData);
                }

                var resultingObject = serializer.Deserialize(new StringReader(xmlData));
                if (resultingObject is T)
                {
                    return (T)resultingObject;
                }
                else
                {
                    throw new Exception($"Deserialized object is not of type {default(T).GetType()}");
                }
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
