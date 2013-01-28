using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe
{
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Class Board - manages the matrix of tiles and the state of the game
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    
    class Board
    {
        private Tile[,] tiles = new Tile[3, 3];

        public void Initialize(ContentManager content, int viewPortWidth, int viewPortHeight)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tiles[i, j] = new Tile();

            // initialize the board tiles           
            tiles[0, 0].Initialize(content, viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight, "BoardTileDarkGray");
            tiles[0, 1].Initialize(content, viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight, "BoardTileLightGray");
            tiles[0, 2].Initialize(content, viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight, "BoardTileDarkGray");
            tiles[1, 0].Initialize(content, viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight, "BoardTileLightGray");
            tiles[1, 1].Initialize(content, viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight, "BoardTileDarkGray");
            tiles[1, 2].Initialize(content, viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight, "BoardTileLightGray");
            tiles[2, 0].Initialize(content, viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight, "BoardTileDarkGray");
            tiles[2, 1].Initialize(content, viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight, "BoardTileLightGray");
            tiles[2, 2].Initialize(content, viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight, "BoardTileDarkGray");
        }

        public void UpdateTilePositions(int viewPortWidth, int viewPortHeight)
        {
            tiles[0, 0].UpdatePosition(viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight);
            tiles[0, 1].UpdatePosition(viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight);
            tiles[0, 2].UpdatePosition(viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.02), viewPortWidth, viewPortHeight);
            tiles[1, 0].UpdatePosition(viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight);
            tiles[1, 1].UpdatePosition(viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight);
            tiles[1, 2].UpdatePosition(viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.35), viewPortWidth, viewPortHeight);
            tiles[2, 0].UpdatePosition(viewPortWidth * ((float)0.20), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight);
            tiles[2, 1].UpdatePosition(viewPortWidth * ((float)0.40), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight);
            tiles[2, 2].UpdatePosition(viewPortWidth * ((float)0.60), viewPortHeight * ((float)0.68), viewPortWidth, viewPortHeight);
        }

        public Winner GetWinner()
        {
            Winner _result = Winner.None;

            //+++++++++++++++++++++++++++++++++++++++++++++++
            // Check to see if Player X has won
            //+++++++++++++++++++++++++++++++++++++++++++++++

            // row 1
            if ((tiles[0, 0].State == TileState.X) && (tiles[0, 1].State == TileState.X) && (tiles[0, 2].State == TileState.X))
                _result = Winner.PlayerX;

            // row 2
            else if ((tiles[1, 0].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[1, 2].State == TileState.X))
                _result = Winner.PlayerX;

            // row 3
            else if ((tiles[2, 0].State == TileState.X) && (tiles[2, 1].State == TileState.X) && (tiles[2, 2].State == TileState.X))
                _result = Winner.PlayerX;

            // col 1
            else if ((tiles[0, 0].State == TileState.X) && (tiles[1, 0].State == TileState.X) && (tiles[2, 0].State == TileState.X))
                _result = Winner.PlayerX;

            // col 2
            else if ((tiles[0, 1].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 1].State == TileState.X))
                _result = Winner.PlayerX;

            // col 3
            else if ((tiles[0, 2].State == TileState.X) && (tiles[1, 2].State == TileState.X) && (tiles[2, 2].State == TileState.X))
                _result = Winner.PlayerX;

            // cross right
            else if ((tiles[0, 0].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 2].State == TileState.X))
                _result = Winner.PlayerX;

            // cross left
            else if ((tiles[0, 2].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 0].State == TileState.X))
                _result = Winner.PlayerX;

            //+++++++++++++++++++++++++++++++++++++++++++++++++
            // Check to see if Player O has won
            //+++++++++++++++++++++++++++++++++++++++++++++++++

            // row 1
            if ((tiles[0, 0].State == TileState.O) && (tiles[0, 1].State == TileState.O) && (tiles[0, 2].State == TileState.O))
                _result = Winner.PlayerO;

            // row 2
            else if ((tiles[1, 0].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[1, 2].State == TileState.O))
                _result = Winner.PlayerO;

            // row 3
            else if ((tiles[2, 0].State == TileState.O) && (tiles[2, 1].State == TileState.O) && (tiles[2, 2].State == TileState.O))
                _result = Winner.PlayerO;

            // col 1
            else if ((tiles[0, 0].State == TileState.O) && (tiles[1, 0].State == TileState.O) && (tiles[2, 0].State == TileState.O))
                _result = Winner.PlayerO;

            // col 2
            else if ((tiles[0, 1].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 1].State == TileState.O))
                _result = Winner.PlayerO;

            // col 3
            else if ((tiles[0, 2].State == TileState.O) && (tiles[1, 2].State == TileState.O) && (tiles[2, 2].State == TileState.O))
                _result = Winner.PlayerO;

            // cross right
            else if ((tiles[0, 0].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 2].State == TileState.O))
                _result = Winner.PlayerO;

            // cross left
            else if ((tiles[0, 2].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 0].State == TileState.O))
                _result = Winner.PlayerO;

            if ((GameState._moves >= 9) && (_result == Winner.None))
                _result = Winner.Draw;

            return _result;
        }

        public void Update()
        {
            // update the state of each tile
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tiles[i, j].Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // draw each tile
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    tiles[i,j].Draw(spriteBatch);
        }

        public void DrawWinLine(ContentManager content, SpriteBatch spriteBatch, int viewportWidth, int viewportHeight)
        {
            //+++++++++++++++++++++++++++++++++++++++++++++++
            // draw the line threw the winning row/col/diag
            //+++++++++++++++++++++++++++++++++++++++++++++++

            // row 1
            if (((tiles[0, 0].State == TileState.X) && (tiles[0, 1].State == TileState.X) && (tiles[0, 2].State == TileState.X)) ||
                ((tiles[0, 0].State == TileState.O) && (tiles[0, 1].State == TileState.O) && (tiles[0, 2].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int)(viewportWidth * .2);
                rect.Y = (int) (tiles[0, 0].Position.Y + viewportHeight * .12);
                rect.Width = (int)(viewportWidth * .58);
                rect.Height = (int)(viewportHeight * .04);
                Texture2D _line = content.Load<Texture2D>("WinLineRow");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // row 2
            else if (((tiles[1, 0].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[1, 2].State == TileState.X)) ||
                     ((tiles[1, 0].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[1, 2].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int) (viewportWidth * .2);
                rect.Y = (int) (tiles[1, 0].Position.Y + viewportHeight * .12);
                rect.Width = (int) (viewportWidth * .58);
                rect.Height = (int)(viewportHeight * .04);
                Texture2D _line = content.Load<Texture2D>("WinLineRow");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // row 3
            else if (((tiles[2, 0].State == TileState.X) && (tiles[2, 1].State == TileState.X) && (tiles[2, 2].State == TileState.X)) ||
                     ((tiles[2, 0].State == TileState.O) && (tiles[2, 1].State == TileState.O) && (tiles[2, 2].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int) (viewportWidth * .2);
                rect.Y = (int) (tiles[2, 0].Position.Y + viewportHeight * .12);
                rect.Width = (int) (viewportWidth * .58);
                rect.Height = (int)(viewportHeight * .04);
                Texture2D _line = content.Load<Texture2D>("WinLineRow");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // col 1
            else if (((tiles[0, 0].State == TileState.X) && (tiles[1, 0].State == TileState.X) && (tiles[2, 0].State == TileState.X)) ||
                     ((tiles[0, 0].State == TileState.O) && (tiles[1, 0].State == TileState.O) && (tiles[2, 0].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int) (tiles[0, 0].Position.X + viewportWidth * .08);
                rect.Y = (int) tiles[0, 0].Position.Y;
                rect.Width = (int) (viewportWidth * .02);
                rect.Height = (int)(viewportHeight * .96);
                Texture2D _line = content.Load<Texture2D>("WinLineCol");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // col 2
            else if (((tiles[0, 1].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 1].State == TileState.X)) ||
                     ((tiles[0, 1].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 1].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int)(tiles[0, 1].Position.X + viewportWidth * .08);
                rect.Y = (int)tiles[0, 1].Position.Y;
                rect.Width = (int)(viewportWidth * .02);
                rect.Height = (int)(viewportHeight * .96);
                Texture2D _line = content.Load<Texture2D>("WinLineCol");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // col 3
            else if (((tiles[0, 2].State == TileState.X) && (tiles[1, 2].State == TileState.X) && (tiles[2, 2].State == TileState.X)) ||
                     ((tiles[0, 2].State == TileState.O) && (tiles[1, 2].State == TileState.O) && (tiles[2, 2].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int) (tiles[0, 2].Position.X + viewportWidth * .08);
                rect.Y = (int)tiles[0, 2].Position.Y;
                rect.Width = (int)(viewportWidth * .02);
                rect.Height = (int)(viewportHeight * .96);
                Texture2D _line = content.Load<Texture2D>("WinLineCol");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // cross right
            else if (((tiles[0, 0].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 2].State == TileState.X)) ||
                     ((tiles[0, 0].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 2].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int)tiles[0, 0].Position.X;
                rect.Y = (int)tiles[0, 0].Position.Y;
                rect.Width = (int)(viewportWidth * .58);
                rect.Height = (int)(viewportHeight * .96);
                Texture2D _line = content.Load<Texture2D>("WinLineDiagTopLeft");
                spriteBatch.Draw(_line, rect, Color.Red);
            }

            // cross left
            else if (((tiles[0, 2].State == TileState.X) && (tiles[1, 1].State == TileState.X) && (tiles[2, 0].State == TileState.X)) ||
                     ((tiles[0, 2].State == TileState.O) && (tiles[1, 1].State == TileState.O) && (tiles[2, 0].State == TileState.O)))
            {
                Rectangle rect = new Rectangle();
                rect.X = (int)tiles[0, 0].Position.X;
                rect.Y = (int)tiles[0, 0].Position.Y;
                rect.Width = (int)(viewportWidth * .58);
                rect.Height = (int)(viewportHeight * .96);
                Texture2D _line = content.Load<Texture2D>("WinLineDiagTopRight");
                spriteBatch.Draw(_line, rect, Color.Red);
            }
        }
    }
}
