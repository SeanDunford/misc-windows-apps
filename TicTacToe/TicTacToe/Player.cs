using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe
{
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Class Player - manages and displays the player name and score
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    
    class Player // base class
    {
        private string _name;
        private Vector2 _position;
        private bool _isPerson;

        public bool IsPerson
        {
            get { return _isPerson; }
            set { _isPerson = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private int _score;
        int _viewportWidth;
        int _viewportHeight;
        private SpriteFont _font;

        public SpriteFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public void Initialize(ContentManager content, string name, float posX, float posY, int viewportWidth, int viewportHeight)
        {
            _score = 0;
            _font = content.Load<SpriteFont>("TicTacToeBuxtonSketch");
            _name = name;
            _position.X = posX;
            _position.Y = posY;
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
        }

        public void Update(int viewportWidth, int viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
        }

        public void Draw(SpriteBatch spriteBatch, double adjust)
        {
            _position.X = ((float)(adjust * _viewportWidth));
            _position.Y = ((float)(0.05 * _viewportHeight));

            // draw the player name and score
            spriteBatch.DrawString(_font, _name, new Vector2(_position.X, _position.Y), Color.Black);
            spriteBatch.DrawString(_font, _score.ToString(), new Vector2(_position.X, _position.Y + 30), Color.Black);
        }
    }

    class SmartPlayer : Player
    {
        private Board _board;

        internal Board Board
        {
            get { return _board; }
            set { _board = value; }
        }

        public SmartPlayer(Board board)
        {
            IsPerson = true;
            _board = board;
        }

        // brute force tic-tac-toe algorithm (if yo ucan call it an algorithm) 
        // should be useing miniMax or somethign like that but I skipped AI
        public void Draw(SpriteBatch spriteBatch, double adjust)
        {
            int Ocount = 0;
            int Xcount = 0;
            int targetCol = -1;
            int targetRow = -1;
            int i, j;

            if (GameState._gameOver == true)
            {
                base.Draw(spriteBatch, adjust);
                return;
            }

            if (IsPerson)
            {
                base.Draw(spriteBatch, adjust);
                return;
            }

            // player is computer so be smart already...
            switch (GameState._moves)
            {
                // opening move
                case 1:
                    if (_board.Tiles[1,1].State == TileState.Blank)                    
                    {
                        // go for the center
                        _board.Tiles[1,1].TileClicked = true;
                        _board.Tiles[1,1].Draw(spriteBatch);
                    }
                    else // go for a corner
                    {
                        if (_board.Tiles[0,0].State == TileState.Blank)
                        {
                            _board.Tiles[0,0].TileClicked = true;
                            _board.Tiles[0,0].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[0,2].State == TileState.Blank)
                        {
                            _board.Tiles[0,2].TileClicked = true;
                            _board.Tiles[0,2].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[2,0].State == TileState.Blank)
                        {
                            _board.Tiles[2,0].TileClicked = true;
                            _board.Tiles[2,0].Draw(spriteBatch);
                        }
                        else
                        {
                            _board.Tiles[2,2].TileClicked = true;
                            _board.Tiles[2,2].Draw(spriteBatch);
                        }
                    }
                    break;

                // looking for 2 'O's in a row or col
                case 3:
                case 5:
                case 7:
                    // check the rows
                    for (i = 0; i < 3; i++)
                    {
                        if (_board.Tiles[i,0].State == TileState.O)
                            Ocount++;
                        if (_board.Tiles[i,1].State == TileState.O)
                            Ocount++;
                        if (_board.Tiles[i,2].State == TileState.O)
                            Ocount++;
                        if (_board.Tiles[i, 0].State == TileState.X)
                            Xcount++;
                        if (_board.Tiles[i, 1].State == TileState.X)
                            Xcount++;
                        if (_board.Tiles[i, 2].State == TileState.X)
                            Xcount++;
                        if ((Ocount >= 2) && (Xcount == 0))
                            break;
                        else
                        {
                            Ocount = 0;
                            Xcount = 0;
                        }
                    }

                    // is there an empty cell on the row with 2 'O's
                    if (Ocount == 2)
                        targetRow = i;
                        
                    if (targetRow != -1)
                    {
                        if (_board.Tiles[targetRow,0].State == TileState.Blank)
                        {
                            _board.Tiles[targetRow,0].TileClicked = true;
                            _board.Tiles[targetRow,0].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[targetRow,1].State == TileState.Blank)
                        {
                            _board.Tiles[targetRow,1].TileClicked = true;
                            _board.Tiles[targetRow,1].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[targetRow,2].State == TileState.Blank)
                        {
                            _board.Tiles[targetRow,2].TileClicked = true;
                            _board.Tiles[targetRow,2].Draw(spriteBatch);
                        }
                    }
                    else
                    {
                        Ocount = 0;
                        for (j = 0; j < 3; j++)
                        {
                            // check the cols
                            if (_board.Tiles[0,j].State == TileState.O)
                                Ocount++;
                            if (_board.Tiles[1,j].State == TileState.O)
                                Ocount++;
                            if (_board.Tiles[2,j].State == TileState.O)
                                Ocount++;
                            if (_board.Tiles[0,j].State == TileState.X)
                                Xcount++;
                            if (_board.Tiles[1,j].State == TileState.X)
                                Xcount++;
                            if (_board.Tiles[2,j].State == TileState.X)
                                Xcount++;
                            if ((Ocount >= 2) && (Xcount == 0))
                                break;
                            else
                            {
                                Ocount = 0;
                                Xcount = 0;
                            }
                        }

                        if (Ocount == 2)
                            targetCol = j;

                        if (targetCol != -1)
                        {
                            if (_board.Tiles[0, targetCol].State == TileState.Blank)
                            {
                                _board.Tiles[0,targetCol].TileClicked = true;
                                _board.Tiles[0,targetCol].Draw(spriteBatch);
                            }
                            else if (_board.Tiles[1,targetCol].State == TileState.Blank)
                            {
                                _board.Tiles[1,targetCol].TileClicked = true;
                                _board.Tiles[1,targetCol].Draw(spriteBatch);
                            }
                            else if (_board.Tiles[2,targetCol].State == TileState.Blank)
                            {
                                _board.Tiles[2,targetCol].TileClicked = true;
                                _board.Tiles[2,targetCol].Draw(spriteBatch);
                            }                        
                        }
                    }

                    // if there are no rows or columns with 2 'O''s and 
                    // go for the side cells and then the corners
                    if ((targetRow == -1) && (targetCol == -1))
                    {
                        if (_board.Tiles[1, 0].State == TileState.Blank)
                        {
                            _board.Tiles[1,0].TileClicked = true;
                            _board.Tiles[1,0].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[1, 2].State == TileState.Blank)
                        {
                            _board.Tiles[1,2].TileClicked = true;
                            _board.Tiles[1,2].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[0,1].State == TileState.Blank)
                        {
                            _board.Tiles[0, 1].TileClicked = true;
                            _board.Tiles[0, 1].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[2, 1].State == TileState.Blank)
                        {
                            _board.Tiles[2, 1].TileClicked = true;
                            _board.Tiles[2, 1].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[0, 0].State == TileState.Blank)
                        {
                            _board.Tiles[0, 0].TileClicked = true;
                            _board.Tiles[0, 0].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[0, 2].State == TileState.Blank)
                        {
                            _board.Tiles[0, 2].TileClicked = true;
                            _board.Tiles[0, 2].Draw(spriteBatch);
                        }
                        else if (_board.Tiles[2, 0].State == TileState.Blank)
                        {
                            _board.Tiles[2, 0].TileClicked = true;
                            _board.Tiles[2, 0].Draw(spriteBatch);
                        }
                        else
                        {
                            _board.Tiles[2, 2].TileClicked = true;
                            _board.Tiles[2, 2].Draw(spriteBatch);
                        }
                    }
                    break;

                default:
                    break;
            }
            base.Draw(spriteBatch, adjust);
        }
    }
}
