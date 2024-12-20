using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace PacMan;

public abstract class Sprite
{
    public const int TailleCase = 50;
    public const int windowWidth = 850;
    public const int windowHeight = 800;
    
    protected Vector2 position;
    protected Texture2D texture;
    
    public bool EstDeplacementValide(Vector2 nextPosition, List<List<int>> grille, Texture2D texture)
    {
        

        int colonneGauche = (int)(nextPosition.X / TailleCase);
        int ligneHaut = (int)(nextPosition.Y / TailleCase);
        int colonneDroite = (int)((nextPosition.X + texture.Width) / TailleCase);  // Utilisation de la taille réelle de la texture
        int ligneBas = (int)((nextPosition.Y + texture.Height) / TailleCase);  // Utilisation de la taille réelle de la texture

        // pour vérifier si Pac-Man ou le fantôme qu ils ne dépassent pas les bords de la fenetre et la grille
        if (nextPosition.X < 0 || nextPosition.X + texture.Width > windowWidth || 
            nextPosition.Y < 0 || nextPosition.Y + texture.Height > windowHeight)
        {
            return false; // cas en dehors des limites de la fenêtre
        }
        if (ligneHaut < 0 || ligneBas >= grille.Count || colonneGauche < 0 || colonneDroite >= grille[0].Count)
        {
            return false; // cas en dehors des limites de la grille
        }

        // pour vérifier que  l'ennemi ne se trouve pas dans un mur (0 = mur)
        for (int i = ligneHaut; i <= ligneBas; i++)
        {
            for (int j = colonneGauche; j <= colonneDroite; j++)
            {
                if (grille[i][j] == 0) // Si la case est un mur
                {
                    return false;
                }
            }
        }

        return true;
    }
    
    public Rectangle GetBoundingBox()
    {
        return new Rectangle((int)position.X + 5, (int)position.Y + 5, texture.Width - 10, texture.Height - 10);
    }
    
    public bool GererCollisionAvec(Sprite other)
    {
        return this.GetBoundingBox().Intersects(other.GetBoundingBox());
    }
    
    
    public void Draw(SpriteBatch spriteBatch, int taille)
    {
        spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, taille, taille), Color.White);
    }
}

