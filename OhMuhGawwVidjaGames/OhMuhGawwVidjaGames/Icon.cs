using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OhMuhGawwVidjaGames
{
    public class Icon: Sprite
    {
     public Icon(Texture2D texture, Vector2 location, Rectangle gameBoundaries)
                : base(texture, location, gameBoundaries)
            {
                
            }
     public override void Update(GameTime gameTime, GameObjects gameObjects)
     {
         base.Update(gameTime, gameObjects);
     }
     protected override void CheckBounds()
     {
        
     }
     public void Draw(SpriteBatch spritebatch)
     {
         spritebatch.Draw(_texture, Location, Color.White);
     }
    }
}
