using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PacMan;

[Serializable]
    public class Pacman : Sprite
    {
        private Texture2D textureOuverte;
        private Texture2D textureFermee;
        
        [XmlElement("PositionX")] public int PositionX { get; set; }
        [XmlElement("PositionY")] public int PositionY { get; set; }

        private int vitesse = 5;



        private float rotationPacman;
        private float animationTimer;
        private float animationIntervalPacman = 0.1f; 
        private bool boucheOuverte = true; // Indicateur pour l'état de la bouche
        private bool isMoving;
        public  static float totalDistance = 0;//pour savoir la distance parcourue

        private List<PointAmanger> points;
        
        [XmlIgnore] public SoundEffect son;
        [XmlIgnore] public SoundEffectInstance instanceSon; 
        private bool sonEnLecture = false;
        
        public void Initialiser(Texture2D textureOuverte, Texture2D textureFermee, SoundEffect son,List<PointAmanger> points) 
        {
            this.textureOuverte = textureOuverte;
            this.textureFermee = textureFermee;
            texture = textureOuverte;
            
            position = new Vector2(PositionX * TailleCase, PositionY * TailleCase);
            
            this.son = son;
            instanceSon = son.CreateInstance();
            instanceSon.IsLooped = true;
            
            this.points = points;
        }
        

        // Mise à jour de l'état de PacMan 
        public virtual void Update(GameTime gameTime, List<List<int>> grille)
        { 

            //Vector2 previousPosition = position;
            Vector2 previousPosition = position;
            Vector2 nextPosition = position;
            
            isMoving = false;
            KeyboardState state = Keyboard.GetState();
            
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.Q))
            {
                nextPosition.X -= vitesse;
                rotationPacman = MathHelper.Pi;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                nextPosition.X += 5;
                rotationPacman = 0f;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.Z))
            {
                nextPosition.Y -= vitesse;
                rotationPacman = -MathHelper.PiOver2;
                isMoving = true;
            }
            if (state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
            {
                nextPosition.Y += vitesse;
                rotationPacman = MathHelper.PiOver2;
                isMoving = true;
            }
            
            if (EstDeplacementValide(nextPosition, grille, texture))
            {
                position = nextPosition;  // le joueur change de position seulement si est un mouvement valide
            }
            
            //mettre a jour l animation et calcul de la distance parcourue
            if (isMoving)
            {
                float distance = Vector2.Distance(previousPosition, nextPosition); // Distance entre l'ancienne et la nouvelle position
                totalDistance += distance; 
                animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (animationTimer >= animationIntervalPacman)
                {
                    boucheOuverte = !boucheOuverte;

                    // Choisir la texture en fonction de l'animation
                    texture = boucheOuverte ? textureOuverte : textureFermee;

                    animationTimer = 0f;
                }
                
                if (!sonEnLecture)
                {
                    instanceSon.Play();
                    sonEnLecture = true;
                }
            }
            else
            {
                if (sonEnLecture)
                {
                    instanceSon.Stop();
                    sonEnLecture = false;
                }
                
                texture = textureOuverte;
                boucheOuverte = true;
                animationTimer = 0f;
            }
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
    
            
            spriteBatch.Draw(texture, new Rectangle((int)position.X + (int)origin.X, (int)position.Y + (int)origin.Y, 30, 30), 
                null, Color.White, rotationPacman, origin, SpriteEffects.None, 0f);
        }
    }
    
