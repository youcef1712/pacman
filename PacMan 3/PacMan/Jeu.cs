using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace PacMan;

[Serializable]
[XmlRoot("Jeu",  Namespace = "http://www.monjeu.com/jeuPacMan")]
public class Jeu
{
    [XmlElement("Carte")] public Carte Carte { get; set; }
    [XmlElement("Pacman")] public Pacman Pacman { get; set; }
    [XmlArray("Ennemis")]
    [XmlArrayItem("Ennemi")] public List<Ennemi> Ennemis { get; set; }
}













