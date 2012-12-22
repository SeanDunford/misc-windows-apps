using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TicTacToe
{
    public enum TileState { Blank = 0, X = 1, O = 2 }
    enum JiggleState { Center = 0, Left = 1, Right = 2 }
    enum JiggleDirection { Left = 0, Right = 1, Stop = 2 }

    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Class Tile - manages the state of a tile and the drawing of graphics to represent that state
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class Tile
    {
        private const int JiggleAmount = 2;

        private Vector2 _position;
        private Texture2D _tileGraphic;
        private Texture2D _X;
        private Texture2D _O;
        SoundEffect _XMoveClip;
        SoundEffect _OMoveClip;
        JiggleState jiggleState;
        JiggleDirection jiggleDirection;

        private int _viewPortWidth;
        private int _viewPortHeight;
        private Vector2 mousePosition = Vector2.Zero;
        private bool _leftMouseClicked = false;
        private bool _tileClicked = false;
        private TileState _state;

        public int ViewPortWidth
        {
            get { return _viewPortWidth; }
            set { _viewPortWidth = value; }
        }

        public int ViewPortHeight
        {
            get { return _viewPortHeight; }
            set { _viewPortHeight = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public bool TileClicked
        {
            get { return _tileClicked; }
            set { _tileClicked = value; }
        }

        public TileState State
        {
            get { return _state; }
            set { _state = value; }
        }

        public void Initialize( ContentManager content, 
                                float X, 
                                float Y, 
                                int viewPortWidth, 
                                int viewPortHeight, 
                                string assetName)
        {
            _state = TileState.Blank;

            _position.X = X;
            _position.Y = Y; 
            _viewPortWidth = viewPortWidth;
            _viewPortHeight = viewPortHeight;

            _tileGraphic = content.Load<Texture2D>(assetName);

            _X = content.Load<Texture2D>("BoardX");
            _O = content.Load<Texture2D>("BoardO");

            _XMoveClip = content.Load<SoundEffect>("XMove");
            _OMoveClip = content.Load<SoundEffect>("OMove");

            jiggleState = JiggleState.Center;
            jiggleDirection = JiggleDirection.Left;
        }

        public void UpdatePosition(float X, float Y, int viewPortWidth, int viewPortHeight)
        {
            _position.X = X;
            _position.Y = Y; 
            _viewPortWidth = viewPortWidth;
            _viewPortHeight = viewPortHeight;
        }
            
        public void Update()
        {
            UpdateMouse();
            CheckMouseClick();

            // if the mouse is clicked on the tile or the user has 
            // touched the tile set the tile clicked flag to true
            if (_leftMouseClicked)
            {
                if (((mousePosition.X) >= _position.X) && (mousePosition.X) <= (_position.X + _tileGraphic.Width) &&
                    (mousePosition.Y) >= _position.Y && (mousePosition.Y) <= (_position.Y + _tileGraphic.Height))
                    _tileClicked = true;
            }
        }

        async public void Draw(SpriteBatch spriteBatch)
        {
            // tile not yet clicked, just draw the tile
            if (_state == TileState.Blank)
            {
                Rectangle rect = new Rectangle((int) _position.X, (int) _position.Y, ((int) (0.18 * ViewPortWidth)), ((int)(0.3 * ViewPortHeight)));
                spriteBatch.Draw(_tileGraphic, rect, Color.White);
            }

            // tile clicked and is currently blank, so set the proper state
            if ((_tileClicked) && (_state == TileState.Blank) && (GameState._gameOver == false))
            {
                _tileClicked = false;

                if (GameState._whosTurn == PlayerTurn.X)
                {
                    _state = TileState.X;
                    GameState._whosTurn = PlayerTurn.O;
                    GameState._moves++;
                    _OMoveClip.Play();
                }
                else
                {
                    _state = TileState.O;
                    GameState._whosTurn = PlayerTurn.X;
                    GameState._moves++;
                    _XMoveClip.Play();
                }
            }

            // if the tile state is set to O or X, then draw the appropriate letter

            if (_state == TileState.O)
            {
                // jiggle the tile
                for (int i = 0; i < JiggleAmount; i++)
                {
                    Jiggle(spriteBatch, _tileGraphic, _position, _O);
                    await Task.Delay(100);
                }
                jiggleDirection = JiggleDirection.Stop;
            }

            if (_state == TileState.X)
            {
                // jiggle the tile
                for (int i = 0; i < JiggleAmount; i++)
                {
                    Jiggle(spriteBatch, _tileGraphic, _position, _X);
                    await Task.Delay(100);
                }
                jiggleDirection = JiggleDirection.Stop;
            }
         }

        void UpdateMouse()
        {
            MouseState mouseState = Mouse.GetState();

            // The mouse X and Y positions are returned relative to the top-left of the game window.
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            // calculate % adjustment to map mouse coordinates into viewport coordinates
            float xAdjust = _viewPortWidth / ((float)GameState._windowsBounds.Width);
            float yAdjust = _viewPortHeight / ((float)GameState._windowsBounds.Height);

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

        void Jiggle(SpriteBatch spriteBatch, Texture2D tile, Vector2 tilePosition, Texture2D letter)
        {
            if (jiggleDirection == JiggleDirection.Stop)
            {
                Rectangle rect = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, ((int)(0.18 * ViewPortWidth)), ((int)(0.3 * ViewPortHeight)));
                spriteBatch.Draw(tile, rect, Color.White);
                spriteBatch.Draw(letter, rect, Color.White);
            }
            else
            {
                switch (jiggleState)
                {
                    case JiggleState.Center:

                        switch (jiggleDirection)
                        {
                            case JiggleDirection.Left:
                                jiggleState = JiggleState.Left;
                                tilePosition.X -= JiggleAmount;
                                tilePosition.Y -= JiggleAmount;
                                break;

                            case JiggleDirection.Right:
                                jiggleState = JiggleState.Right;
                                tilePosition.X += JiggleAmount;
                                tilePosition.Y += JiggleAmount;
                                break;
                        }
                        break;

                    case JiggleState.Left:

                        switch (jiggleDirection)
                        {
                            case JiggleDirection.Left:
                                jiggleDirection = JiggleDirection.Right;
                                break;

                            case JiggleDirection.Right:
                                jiggleState = JiggleState.Center;
                                tilePosition.X += JiggleAmount;
                                tilePosition.Y += JiggleAmount;
                                break;
                        }
                        break;

                    case JiggleState.Right:

                        switch (jiggleDirection)
                        {
                            case JiggleDirection.Left:
                                jiggleState = JiggleState.Center;
                                tilePosition.X -= JiggleAmount;
                                tilePosition.Y -= JiggleAmount;
                                break;

                            case JiggleDirection.Right:
                                jiggleDirection = JiggleDirection.Left;
                                break;
                        }
                        break;
                }

                Rectangle rect = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, ((int)(0.18 * ViewPortWidth)), ((int)(0.3 * ViewPortHeight)));
                spriteBatch.Draw(tile, rect, Color.White);
                spriteBatch.Draw(letter, rect, Color.White);
            }
        }
    }
}

