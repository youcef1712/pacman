using System.IO;
using System.Xml.Serialization;

namespace PacMan;
public static class XmlLoader
{
    //classe pour la sérialisation et désérialisation

    public static void Save(string path, object obj, XmlSerializerNamespaces ns) {
        using (TextWriter writer = new StreamWriter(path))
        {
            var xml = new XmlSerializer(obj.GetType());
            xml.Serialize(writer, obj, ns);
        }
    }

    public static T LoadFromXml<T>(string filePath)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            return (T)serializer.Deserialize(stream);
        }
    }
}