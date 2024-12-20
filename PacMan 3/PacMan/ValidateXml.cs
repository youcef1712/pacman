using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace PacMan;

public class XmlValidation
{
    //classe pour validation du xml 
    public static async Task ValidateXmlFileAsync(string schemaNamespace, string xsdFilePath, string xmlFilePath)
    {
        var settings = new XmlReaderSettings();
        settings.Schemas.Add(schemaNamespace, xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        Console.WriteLine("Nombre de schémas utilisés dans la validation : " + settings.Schemas.Count);
        settings.ValidationEventHandler += ValidationCallBack;

        using (var readItems = XmlReader.Create(xmlFilePath, settings))
        {
            while (readItems.Read())
            {
            }
        }
    }

    private static void ValidationCallBack(object? sender, ValidationEventArgs e)
    {
        if (e.Severity.Equals(XmlSeverityType.Warning))
        {
            Console.Write("WARNING: ");
            Console.WriteLine(e.Message);
        }
        else if (e.Severity.Equals(XmlSeverityType.Error))
        {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);
        }
    }
}