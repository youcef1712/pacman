using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan.Content;

public class listePoints
{
    private List<List<int>> grille; 
    private List<PointAmanger> pointsAmanger;  
    private Texture2D texturePointAmanger;  
    public listePoints(List<List<int>> grille, Texture2D texturePointAmanger)
    {
        this.grille = grille;
        this.texturePointAmanger = texturePointAmanger;
        pointsAmanger = new List<PointAmanger>();
        GenererPointsAmanger();
    }  
    
    public List<PointAmanger> GetListePoint()
    {
        return pointsAmanger;
    }

    
    private void GenererPointsAmanger()
    {
        for (int i = 0; i < grille.Count; i++)
        {
            for (int j = 0; j < grille[i].Count; j++)
            {
                //le cas ou la case est valide(1) on met un point
                if (grille[i][j] == 1)
                {
                    // Convertir la position de la case en coordonnées de l'écran
                    Vector2 positionPoint = new Vector2(j * 50 + 17, i * 50 + 17);  // Ajuster la position du point dans la case
                    PointAmanger point = new PointAmanger(positionPoint, texturePointAmanger);
                    pointsAmanger.Add(point);
                }
            }
        }
    }

    
    public void Draw(SpriteBatch spriteBatch, int taille)
    {
        foreach (var point in pointsAmanger)
        {
            point.Draw(spriteBatch, taille);  // Dessiner chaque point à manger
        }
    }

}