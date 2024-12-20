using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan;

public class FormulaireJoueur
{
    //variable pour recuperer le nom , age et prenom

    private string JoueurNom = "";
    private string JoueurPrenom = "";
    private int JoueurAge = 0;
    
    //boolean pour s avoir si le joueur a entré un nom prenom ou age
    private bool JAEntreNom = false;
    private bool JAEntrePrenom = false;
    private bool JAEntreAge = false;

    private SpriteFont font;
    private KeyboardState previousState; 

    public FormulaireJoueur(SpriteFont font)
    {
        this.font = font;
    }

   public DonneesJoueur RecupererDonneesJoueur(KeyboardState currentState)
{
    foreach (Keys key in Enum.GetValues(typeof(Keys)))
    {
        
        if (currentState.IsKeyDown(key) && !previousState.IsKeyDown(key))
        {
            // Le joueur a entré un nom
            if (!JAEntreNom && key != Keys.Back && key != Keys.Enter)
            {
                char c = ConvertKeyToChar(key);
                if (char.IsLetterOrDigit(c))
                {
                    JoueurNom += c;
                }
            }
            // Le joueur a entré un prenom
            else if (!JAEntrePrenom && key != Keys.Back && key != Keys.Enter)
            {
                char c = ConvertKeyToChar(key);
                if (char.IsLetterOrDigit(c))
                {
                    JoueurPrenom += c;
                }
            }
            // Le joueur a entré son age
            else if (!JAEntreAge && JAEntrePrenom)
            {
                
                if (key == Keys.Up)
                {
                    JoueurAge++;
                }
                if (key == Keys.Down && JoueurAge > 0)
                {
                    JoueurAge--;
                }
            }

            // Gestion de la suppression 
            if (key == Keys.Back)
            {
                if (!JAEntreNom && JoueurNom.Length > 0)
                {
                    JoueurNom = JoueurNom.Remove(JoueurNom.Length - 1);
                }
                else if (!JAEntrePrenom && JoueurPrenom.Length > 0)
                {
                    JoueurPrenom = JoueurPrenom.Remove(JoueurPrenom.Length - 1);
                }
            }

            // pour valider en tappant entrer
            if (key == Keys.Enter)
            {
                if (!JAEntreNom)
                {
                    JAEntreNom = true;
                }
                else if (!JAEntrePrenom)
                {
                    JAEntrePrenom = true;
                }
                else if (!JAEntreAge)
                {
                    JAEntreAge = true;
                }
            }
        }
    }

    
    previousState = currentState;

    //Si le joueur a tout rempli ont le retourne dans données des joueurs 
    if (JAEntreAge)
    {
        return new DonneesJoueur(JoueurNom, JoueurPrenom, JoueurAge);
    }

    return null;
}

    // Convertir la touche en caractère
    private char ConvertKeyToChar(Keys key)
    {
        
        if (key >= Keys.A && key <= Keys.Z)
        {
            
            return (char)(key - Keys.A + 'A');
        }

        if (key >= Keys.D0 && key <= Keys.D9)
        {
            
            return (char)(key - Keys.D0 + '0');
        }

        return '\0'; 
    }

    
    private string CleanString(string input)
    {
        return new string(input.Where(c => font.Characters.Contains(c)).ToArray());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        string cleanedNom = CleanString(JoueurNom);
        string cleanedPrenom = CleanString(JoueurPrenom);

        // Afficher le formulaire
        spriteBatch.DrawString(font, "Nom : " + cleanedNom, new Vector2(100, 100), Color.White);

        if (JAEntreNom)
        {
            spriteBatch.DrawString(font, "Prénom : " + cleanedPrenom, new Vector2(100, 150), Color.White);
        }

        if (JAEntrePrenom)
        {
            spriteBatch.DrawString(font, "Age : " + JoueurAge.ToString(), new Vector2(100, 200), Color.White);
        }
    }
}
