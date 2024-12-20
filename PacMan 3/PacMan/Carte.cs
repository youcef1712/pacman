using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan;

[Serializable]
public class Carte : Sprite
{
    [XmlElement("Ligne")] public List<string> Lignes { get; set; }
    private List<List<int>> grille;
    
    public void Initialiser(Texture2D texture)
    {
        this.texture = texture;
        grille = new List<List<int>>();
    }
    //retourner la grille

    public List<List<int>> GetGrille()
    {
        return grille;
    }
    
    public void ConvertirLignesEnGrille()
    {
        grille.Clear();
        foreach (var ligne in Lignes)
        {
            var ligneMap = new List<int>();
            foreach (var c in ligne.Split(' '))
            {
                ligneMap.Add(int.Parse(c));
            }
            grille.Add(ligneMap);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < grille.Count; i++)
        {
            for (int j = 0; j < grille[i].Count; j++)
            {
                int caseValue = grille[i][j];
                if (caseValue == 0) // si c'est un mur
                {
                    spriteBatch.Draw(texture, new Rectangle(j * TailleCase , i * TailleCase, TailleCase, TailleCase), Color.White);
                }
            }
        }
    }
}


    
    


