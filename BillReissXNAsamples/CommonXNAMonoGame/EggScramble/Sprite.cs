using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{
    class Sprite
    {
        public Vector2 Position;
        public float Speed;
        public Texture2D Texture;
        public Vector2 Size;

        public virtual Vector2 Center
        {
            get
            {
                return Position + (Size / 2); 
            }
        }

        public virtual void Draw(ScaledSpriteBatch scaledSpriteBatch)
        {
            scaledSpriteBatch.Draw(Texture, Position, Size);
        }

        public virtual void Update(double elapsed)
        {
        }
    }
}
