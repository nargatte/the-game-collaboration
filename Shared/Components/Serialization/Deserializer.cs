using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Components.Serialization
{
    public class Deserializer
    {
        public static T Deserialize<T>(string serializedObject)
            where T: class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            Stream s = GenerateStreamFromString(serializedObject);
            XmlReader reader = new XmlTextReader(s);

            if (!serializer.CanDeserialize(reader))
            {
                s.Dispose();
                reader.Close();
                return null;
            }

            var o = (T) serializer.Deserialize(reader);
            s.Dispose();
            reader.Close();

            return o;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}