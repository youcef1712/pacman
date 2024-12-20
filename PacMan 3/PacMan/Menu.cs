using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PacMan;

public class Menu
{
    private SpriteFont font;
    private string[] options; 
    private int selectedOption; 
    private KeyboardState previousKeyboardState;
    private bool isGameOver; 
    public Menu()
    {
        options = new string[] { "Jouer", "Quitter" }; 
        selectedOption = 0; 
        isGameOver = false; 
    }

    //menu avec rejouer et quitter dans le cas ou il pert ou gagne
    public void SetGameOver(bool gameOver)
    {
        isGameOver = gameOver;
        // Si le jeu est terminé, on affiche "Rejouer" ou "Quitter"
        if (isGameOver)
        {
            options = new string[] { "Rejouer", "Quitter" }; 
            selectedOption = 0; 
        }
        else
            //sinon on affiche le menu debut du jeu

        {
            options = new string[] { "Jouer", "Quitter" }; 
            selectedOption = 0;
        }
    }

    public int SelectedOption => selectedOption;

    public void LoadContent(ContentManager content)
    {
        font = content.Load<SpriteFont>("File"); 
    }
    
    public void Update()
    {
        KeyboardState state = Keyboard.GetState();

        
        if (state.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
            selectedOption = (selectedOption - 1 + options.Length) % options.Length; // Remonte dans le menu
        if (state.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
            selectedOption = (selectedOption + 1) % options.Length; // Descend dans le menu

        
        previousKeyboardState = state;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, Color normalColor, Color selectedColor)
    {
        for (int i = 0; i < options.Length; i++)
        {
            Color color = (i == selectedOption) ? selectedColor : normalColor;
            Vector2 textPosition = new Vector2(position.X, position.Y + i * 30); // Décale chaque option verticalement
            spriteBatch.DrawString(font, options[i], textPosition, color);
        }
    }
}
