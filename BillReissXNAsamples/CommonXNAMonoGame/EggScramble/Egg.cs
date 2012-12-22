using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{
    class Egg : Sprite
    {
        public static Vector2 EggSize = new Vector2(32, 40);
        public Rectangle CollisionRect = new Rectangle(6, 6, 26, 34);

        public Egg(Vector2 pos, Texture2D texture, Vector2 speed)
        {
            Position = pos;
            Texture = texture;
            Speed = speed.Y;
            Size = new Vector2(32, 40);
        }

        public void Update(double elapsed, Chicken chicken, out bool collidesWithChicken)
        {
            Position.Y += Speed * (float)elapsed;
            if (Position.Y + Size.Y > chicken.Position.Y)
            {
                Rectangle c1 = this.CollisionRect;
                c1.Offset((int)Position.X, (int)Position.Y);
                Rectangle c2 = chicken.CollisionRect;
                c2.Offset((int)chicken.Position.X, (int)chicken.Position.Y);
                collidesWithChicken = c1.Intersects(c2);
            }
            else
            {
                collidesWithChicken = false;
            }
        }

        public override void Update(double elapsed)
        {
            base.Update(elapsed);
        }
    }
}
