using System;
using System.Threading.Tasks;
using PacMan;
using static PacMan.XmlValidation;

public static partial class Program
{
    static async Task Main()
    {
        
        string xmlPath = "../../../xml/PacMan.xml";  
        string xsdPath = "../../../xsd/PacMan.xsd"; // 
        string schemaNamespace = "http://www.monjeu.com/jeuPacMan"; // Namespace

        // Validation du fichier XML pacman
        Console.WriteLine("Validation du fichier Pacman...");
        await ValidateXmlFileAsync(schemaNamespace, "../../../xsd/PacMan.xsd", "../../../xml/PacMan.xml");



        using (var game = new PacMan.Game1())
        {
            game.Run();
        }
        //validation du fichier partie.xml
        Console.WriteLine("Validation du fichier partie.xml...");
       await ValidateXmlFileAsync("http://www.monjeu.com/partie", "../../../xsd/Partie.xsd", "../../../XmlGenerer/Partie.xml");
       //Validaiton du fichier Sauvegarde.xml
        Console.WriteLine("Validation du fichier sauvegarde.xml...");
        await ValidateXmlFileAsync("http://www.monjeu.com/jeuPacMan/sauvegarde", "../../../xsd/Sauvegarde.xsd", "../../../XmlGenerer/Sauvegarde.xml");
    }
}
