using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan;

[Serializable]
public class Ennemi : Sprite
{
    // Sérialiser les positions du fantome

    [XmlElement("PositionX")] public int PositionX { get; set; }

    [XmlElement("PositionY")] public int PositionY { get; set; }
    
    private Random random = new Random();
    private float animationTimer;
    private float animationInterval = 0.1f;
    
    private Vector2 direction;
    
    public void Initialiser(Texture2D texture)
    {
        this.texture = texture;
        position = new Vector2(PositionX * TailleCase, PositionY * TailleCase);
        direction = ChoisirNouvelleDirection();
        random = new Random();
    }

    public void Update(GameTime gameTime, List<List<int>> grille)
    {
        Vector2 nextPosition = position + direction;
        while (!EstDeplacementValide(nextPosition, grille, texture))
        {
            direction = ChoisirNouvelleDirection();  // dans le cas ou il recontre un mur il change de direction
            nextPosition = position + direction;    // Recalculer la nouvelle position
        } 
        
        position = nextPosition;

        // Mise à jour de l'animation (si nécessaire)
        animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (animationTimer >= animationInterval)
        {
            // Animation ou autres comportements
            animationTimer = 0f;
        }
    }

    private Vector2 ChoisirNouvelleDirection()
    {
        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, -3),  // Haut
            new Vector2(0, 3),   // Bas
            new Vector2(-3, 0),  // Gauche
            new Vector2(3, 0)    // Droite
        };

        
        return directions[random.Next(directions.Length)];
    }
}