using System;
using System.IO;
using System.Xml;

namespace PacMan
{
    public class SauvegardePartie
    {
        //fonction qui ajoute la partie dans le fichier sauvegarde.xml

        public static void AjouterPartie(string nomJoueur, int nbParties, DateTime date, string etatJeu, int score)
        {
            string filePath = "../../../XmlGenerer/Sauvegarde.xml";
            string namespaceUri = "http://www.monjeu.com/jeuPacMan/sauvegarde";

            XmlDocument doc = new XmlDocument();
            XmlElement root;

            //pour voir si le fichier existe ou on le crée
            if (File.Exists(filePath))
            {
                doc.Load(filePath);
                root = doc.DocumentElement;
            }
            else
            {
                root = doc.CreateElement("Sauvegarde", namespaceUri);
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
                doc.AppendChild(root);
            }

            // Vérifier si le joueur existe alors il cree le joueur avec la partie sinon il ajoute juste la partie 
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("ns", namespaceUri);
            XmlNode joueurNode = root.SelectSingleNode($"ns:Joueur[ns:Nom='{nomJoueur}']", nsManager);
            if (joueurNode == null)
            {
                joueurNode = CreerJoueur(doc, nomJoueur, namespaceUri);
                root.AppendChild(joueurNode);
            }

            //augmenter le nombre de partie du joueur 
            XmlElement nbPartieNode = joueurNode["NbPartie"];
            int nbPartiesActuelles = int.Parse(nbPartieNode.InnerText) + 1;
            nbPartieNode.InnerText = nbPartiesActuelles.ToString();

            // Ajout de la partie
            XmlElement partiesNode = joueurNode["Parties"];
            XmlElement nouvellePartie = CreerPartie(doc, partiesNode, date, etatJeu, score, namespaceUri);
            partiesNode.AppendChild(nouvellePartie);

            
            doc.Save(filePath);
            Console.WriteLine($"Partie ajoutée : Joueur={nomJoueur}, ID={nouvellePartie["ID"].InnerText}");
        }
        // fonction CreerJoueur et CreerPartie utilisé dans Ajoutpartie

        private static XmlNode CreerJoueur(XmlDocument doc, string nomJoueur, string namespaceUri)
        {
            XmlElement joueurNode = doc.CreateElement("Joueur", namespaceUri);
            joueurNode.AppendChild(CreerElement(doc, "Nom", nomJoueur, namespaceUri));
            joueurNode.AppendChild(CreerElement(doc, "NbPartie", "0", namespaceUri));
            joueurNode.AppendChild(doc.CreateElement("Parties", namespaceUri));
            return joueurNode;
        }

        private static XmlElement CreerPartie(XmlDocument doc, XmlElement partiesNode, DateTime date, string etatJeu, int score, string namespaceUri)
        {
            // Générer l'ID unique partie du joueur qui s incrémente a chaque partie 
            int dernierID = 0;
            foreach (XmlNode partie in partiesNode.ChildNodes)
            {
                int id = int.Parse(partie["ID"].InnerText);
                if (id > dernierID) dernierID = id;
            }
            int idPartie = dernierID + 1;

            // Créer la partie 
            XmlElement partieNode = doc.CreateElement("Partie", namespaceUri);
            partieNode.AppendChild(CreerElement(doc, "ID", idPartie.ToString("D3"), namespaceUri));
            partieNode.AppendChild(CreerElement(doc, "Date", date.ToString("yyyy-MM-dd"), namespaceUri));
            partieNode.AppendChild(CreerElement(doc, "EtatJeu", etatJeu, namespaceUri));
            partieNode.AppendChild(CreerElement(doc, "Score", score.ToString(), namespaceUri));
            return partieNode;
        }

        private static XmlElement CreerElement(XmlDocument doc, string nom, string valeur, string namespaceUri)
        {
            XmlElement element = doc.CreateElement(nom, namespaceUri);
            element.InnerText = valeur;
            return element;
        }
    }
}
