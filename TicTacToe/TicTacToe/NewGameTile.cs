using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TicTacToe
{
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Class NewGameTile - special board tile that asks of the players if they want to play again
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    class NewGameTile
    {
        private Vector2 _position;
        private Texture2D _newGameButton;
        private int _viewportWidth;
        private int _viewportHeight;
        private Vector2 mousePosition = Vector2.Zero;
        private bool _leftMouseClicked = false;
        private bool _buttonClicked = false;

        public bool ButtonClicked
        {
            get { return _buttonClicked; }
            set { _buttonClicked = value; }
        }

        public void Initialize(ContentManager content, int viewPortWidth, int viewPortHeight)
        {
            _viewportWidth = viewPortWidth;
            _viewportHeight = viewPortHeight;

            _position.X = _viewportWidth * ((float)0.80);
            _position.Y = _viewportHeight * ((float)0.68);

            _newGameButton = content.Load<Texture2D>("NewGameButton");
        }

        public void Update(int viewPortWidth, int viewPortHeight)
        {
            UpdateMouse();
            CheckMouseClick();

            _viewportWidth = viewPortWidth;
            _viewportHeight = viewPortHeight;

            _position.X = _viewportWidth * ((float)0.80);
            _position.Y = _viewportHeight * ((float)0.68);

            if (_leftMouseClicked)
            {
                if (((mousePosition.X) >= _position.X) && (mousePosition.X) <= (_position.X + _newGameButton.Width) &&
                    (mousePosition.Y) >= _position.Y && (mousePosition.Y) <= (_position.Y + _newGameButton.Height))
                    _buttonClicked = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GameState._gameOver == true)
            {
                Rectangle rect = new Rectangle((int)_position.X, (int)_position.Y, ((int)(0.18 * _viewportWidth)), ((int)(0.3 * _viewportHeight)));
                spriteBatch.Draw(_newGameButton, rect, Color.White);
            }
         }

        void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            // The mouse X and Y positions are returned relative to the top-left of the game window.
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            // calculate % adjustment to map mouse coordinates into viewport coordinates
            float xAdjust = _viewportWidth / ((float)GameState._windowsBounds.Width);
            float yAdjust = _viewportHeight / ((float)GameState._windowsBounds.Height);

            // adjust the mouse coodinates from screen to viewport
            mousePosition.X *= xAdjust;
            mousePosition.Y *= yAdjust;
        }

        void CheckMouseClick()
        {
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                _leftMouseClicked = true;
            else _leftMouseClicked = false;
        } 
    }
}
