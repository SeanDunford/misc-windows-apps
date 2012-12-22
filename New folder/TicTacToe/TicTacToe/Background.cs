using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe
{
    class Background
    {
        //Texture2D _backgroundImage;
        int _viewportWidth;
        int _viewportHeight;

        public void Inititalize(ContentManager content, int viewportWidth, int viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;

            // this is a picture that can ube used as background
            // for a more 'metro' (am I allowed to say that in code?) look
            // I am going to use a whote background for now
            //_backgroundImage = content.Load<Texture2D>("TicTacToeBackground");
        }

        public void Update(int viewportWidth, int viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice gd)
        {
            //Rectangle rect = new Rectangle();
            //rect.X = 0;
            //rect.Y = 0;
            //rect.Width = _viewportWidth;
            //rect.Height = _viewportHeight;
            //spriteBatch.Draw(_backgroundImage, rect, Color.White);

            // white background
            gd.Clear(Color.White);
        }
    }
}
