using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input; 


namespace OhMuhGawwVidjaGames
{
    public abstract class Sprite
    {
        protected readonly Texture2D _texture;
        public Vector2 Location;
        protected Rectangle gameBoundaries;
        public Rectangle boundingBox
        {
            get { return new Rectangle((int)Location.X, (int)Location.Y, Width, Height); }  
        }
        public Vector2 Velocity
        {
            get;
            protected set;
        }
        public int Width
        {
            get { return _texture.Width; }
        }
        public int Height
        {
            get { return _texture.Height; }
        }

        public Sprite(Texture2D texture, Vector2 location, Rectangle gameBoundaries)
        {
            this._texture = texture;
            this.Location = location;
            this.Velocity = Vector2.Zero;
            this.gameBoundaries = gameBoundaries; 
 
            
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(_texture, Location, Color.White);

        }
        public virtual void Update(GameTime gameTime, GameObjects gameObjects)
        {
            Location += Velocity;
            CheckBounds();

        }

        protected abstract void CheckBounds();
    }
  

}