using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PacMan;

public class PointAmanger : Sprite
{
    
    public PointAmanger(Vector2 position, Texture2D texture)
    {
        this.texture = texture;
        this.position = position;
    }
    
}
