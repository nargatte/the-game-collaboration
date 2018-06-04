using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace Shared.Components.ArgumentOptions
{
    public static class ArgumentOptionsHelper
    {
        public static Dictionary<string, string> GetDictonary(string[] args)
        {
            var d = new Dictionary<string,string>();
            string newArg = null;
            for (int i = 0; i < args.Length; i++)
            {
                if (newArg != null)
                {
                    d.Add(newArg, args[i]);
                    newArg = null;
                }
                else
                {
                    if (args[i].StartsWith("--"))
                    {
                        newArg = args[i].Substring(2);
                    }
                }
            }

            return d;
        }

        public static T GetConfigFile<T>(string path)
        {
            XmlSerializer serializer = new
                XmlSerializer(typeof(T));

            // A FileStream is needed to read the XML document.
            FileStream fs = new FileStream(path, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            // Declare an object variable of the type to be deserialized.
            T i;

            // Use the Deserialize method to restore the object's state.
            i = (T)serializer.Deserialize(reader);

            fs.Close();

            return i;
        }
    }
}