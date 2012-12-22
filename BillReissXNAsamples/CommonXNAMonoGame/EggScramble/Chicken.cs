using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{
    class Chicken : Sprite
    {
        public Rectangle CollisionRect = new Rectangle(4, 24, 67, 28);
        float maxX;
        float minX;

        public Chicken()
        {
            Size = new Vector2(75, 117);
            maxX = 750 - Size.X;
            minX = 50;
        }

        public Vector2 Velocity = Vector2.Zero;

        public override void Update(double elapsed)
        {
            base.Update(elapsed);
            Position += Velocity * (float)elapsed;
            Position.X = Math.Max(Math.Min(Position.X, maxX), minX);
        }
    }
}
