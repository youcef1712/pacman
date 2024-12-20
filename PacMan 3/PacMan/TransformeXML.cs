using System;
using System.Xml;
using System.Xml.Xsl;

namespace PacMan;

public class TransformeXML
{
    public static void TransformerXMLVersHTML(string xmlPath, string xsltPath, string outputPath) 
    {
        try
        {
            XslCompiledTransform xslt = new XslCompiledTransform();

            // Charger la feuille XSLT
            Console.WriteLine("Chargement");
            xslt.Load(xsltPath);

            // Appliquer la transformation
            Console.WriteLine("Transformation en cours.");
            xslt.Transform(xmlPath, outputPath);

            Console.WriteLine($"Transformation terminée  Le fichier HTML est généré : {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la transformation : {ex.Message}");
        }
    }

    // méthode pour transformer un fichier XML vers un autre fichier XML en réglant le probleme de l'indentation
    public static void TransformerXMLVersXML(string xmlPath, string xsltPath, string outputPath)
    {
        try
        {
            XslCompiledTransform xslt = new XslCompiledTransform();

            
            Console.WriteLine("Chargement");
            xslt.Load(xsltPath);

            // Configurer XmlWriterSettings pour régler indentation
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true, 
                IndentChars = "  ", 
                NewLineOnAttributes = false, 
                Encoding = new System.Text.UTF8Encoding(false) 
            };

            // Appliquer la transformation en utilisant XmlWriter avec les paramètres d'indentation
            using (XmlWriter writer = XmlWriter.Create(outputPath, settings))
            {
                Console.WriteLine("Transformation en cours");
                xslt.Transform(xmlPath, writer);
            }

            Console.WriteLine($"Transformation terminée Le fichier XML est généré : {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la transformation : {ex.Message}");
        }
    }

}