using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{
    class ScaledSpriteBatch
    {
        SpriteBatch spriteBatch;
        
        public ScaledSpriteBatch(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        public float TextScale = 1;
        public float GlobalScale = 1;
        public Vector2 Offset = Vector2.Zero;

        public void Begin()
        {
            Matrix m = Matrix.CreateTranslation(new Vector3(Offset.X, Offset.Y, 0));
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, m);
        }

        public void Draw(Texture2D texture, Vector2 position, Vector2 size)
        {
            Vector2 localScale = new Vector2(size.X / texture.Width, size.Y / texture.Height);
            spriteBatch.Draw(texture, position * GlobalScale, null, Color.White, 0, Vector2.Zero, localScale * GlobalScale, SpriteEffects.None, 0);  
        }

        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            Vector2 pos = new Vector2((int)(position.X * GlobalScale), (int)(position.Y * GlobalScale));
            spriteBatch.DrawString(font, text, pos, color, 0, Vector2.Zero, GlobalScale * TextScale, SpriteEffects.None, 0);
        }

        public Vector2 MeasureString(SpriteFont font, string text)
        {
            return font.MeasureString(text) * TextScale;
        }
    }
}
