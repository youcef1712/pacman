using System;
using System.Xml.Serialization;

namespace PacMan;
//classe DonneesJoueur Sérialiser

[Serializable]
public class DonneesJoueur
{
    [XmlElement("Nom")] public string Nom { get; set; }
    [XmlElement("Prenom")] public string Prenom { get; set; }
    [XmlElement("Age")] public int Age { get; set; }

    
    public DonneesJoueur(string nom, string prenom, int age)
    {
        this.Nom = nom;
        this.Prenom = prenom;
        this.Age = age;
    }

    
    public DonneesJoueur()
    {
    }
}