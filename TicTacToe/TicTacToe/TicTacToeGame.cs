using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace TicTacToe
{
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Class TicTacToe - main game class intializes game, manages subclasses (board and players), 
    //     resets game when appropriate
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public class TicTacToe : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SoundEffect WinnerClip;
        SoundEffect DrawClip;

        Background background;
        Board board;
        SmartPlayer playerX;
        SmartPlayer playerO;
        NewGameTile newGameTile;

        MessageDialog gameType;

        public TicTacToe() : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            background = new Background();
            board = new Board();
            playerX = new SmartPlayer(board);
            playerO = new SmartPlayer(board);
            newGameTile = new NewGameTile();

            GameState.Initialize();
            GameState._whosTurn = PlayerTurn.O; // should make this random
            GameState._gameOver = false;

            IsMouseVisible = true;

            gameType = new MessageDialog("One player or two player?");

            base.Initialize();
        }

        private void GameTypeDialog()
        {
            gameType.Title = "Tic Tac Toe";

            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            gameType.Commands.Add(new UICommand("One player", new UICommandInvokedHandler(this.OnSetGameType)));
            gameType.Commands.Add(new UICommand("Two player", new UICommandInvokedHandler(this.OnSetGameType)));

            // Set the command that will be invoked by default
            gameType.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            gameType.CancelCommandIndex = 0;
        }

        private void OnSetGameType(IUICommand command)
        {
            if (command.Label.Equals("Two player"))
                playerX.IsPerson = true;
            else
                playerX.IsPerson = false;
        }

        public async void ShowGameTypeDialog()
        {
            await gameType.ShowAsync();
        }

        protected void ResetGame()
        {
            newGameTile.ButtonClicked = false;

            board = null;
            board = new Board();
            playerX.Board = board;
            board.Initialize(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            GameState._whosTurn = PlayerTurn.O; // should make this random
            GameState._gameOver = false;
            GameState._moves = 0;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            background.Inititalize(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            board.Initialize(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            playerX.Initialize(Content, "Player X", ((float)0.05 * GraphicsDevice.Viewport.Width), ((float)0.05 * GraphicsDevice.Viewport.Height), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            playerO.Initialize(Content, "Player O", ((float) 0.83 * GraphicsDevice.Viewport.Width), ((float) 0.05 * GraphicsDevice.Viewport.Height), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            newGameTile.Initialize(Content, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            WinnerClip = Content.Load<SoundEffect>("Winner");
            DrawClip = Content.Load<SoundEffect>("Draw");

            GameTypeDialog();
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // if this is our first time in, ask the user what type of game it is, one player or two
            if (GameState._startup)
            {
                ShowGameTypeDialog();
                GameState._startup = false;
            }

            // check to see if we have a winner yet
            Winner winner = board.GetWinner();

            background.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            board.UpdateTilePositions(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            switch (winner)
            {
                case Winner.None:
                    // game is still in play
                    board.Update();
                    playerO.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    playerX.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    break;
                
                case Winner.Draw:
                    // game over, draw
                    if (GameState._gameOver == false)
                        DrawClip.Play();

                    // now set the game state to over
                    GameState._gameOver = true;

                    // now there is a tile in the lower right 
                    // asking is the players want to play again
                    newGameTile.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    break;

                case Winner.PlayerO:

                    if (GameState._gameOver == false)
                    {
                        // increment the player score if game is just ending
                        playerO.Score = playerO.Score + 1;

                        // play the winner sound
                        WinnerClip.Play();
                    }

                    // now set the game state to over
                    GameState._gameOver = true;

                    // now there is a tile in the lower right 
                    // asking is the players want to play again
                    newGameTile.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    break;

                case Winner.PlayerX:

                    if (GameState._gameOver == false)
                    {
                        // increment the player score if game is just ending
                        playerX.Score = playerX.Score + 1;

                        // play the winner sound
                        WinnerClip.Play();
                    }

                    // now set the game state to over
                    GameState._gameOver = true;

                    // now there is a tile in the center 
                    // asking is the players want to play again
                    newGameTile.Update(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected async override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // Start drawing
            spriteBatch.Begin();

            switch (GameState._windowState)
            {
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // window is snapped to the left or right and is only taking 25% of the screen
                // so just display the logo and the players scores
                //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                case WindowState.Snap1Quarter:

                    SpriteFont _font = Content.Load<SpriteFont>("TicTacToeBuxtonSketch");
    
                    // draw the logo tile
                    Rectangle rect = new Rectangle(((int) (GraphicsDevice.Viewport.Width * 0.22)),
                                                   ((int) (GraphicsDevice.Viewport.Height * 0.22)),
                                                   ((int) (GraphicsDevice.Viewport.Width * 0.6)),
                                                   ((int) (GraphicsDevice.Viewport.Width * 0.6)));
                    Texture2D logo = Content.Load<Texture2D>("TicTacToeLogo");
                    spriteBatch.Draw(logo, rect, Color.White);

                    // draw the player scores
                    Vector2 vect = new Vector2(((float) (GraphicsDevice.Viewport.Width * 0.22)), ((float) (GraphicsDevice.Viewport.Height * 0.5)));
                    spriteBatch.DrawString(_font, playerX.Name, vect, Color.Black);
                    vect.Y = vect.Y + 30;
                    spriteBatch.DrawString(_font, playerX.Score.ToString(), vect, Color.Black);
                    vect.Y = vect.Y + 50;
                    spriteBatch.DrawString(_font, playerO.Name, vect, Color.Black);
                    vect.Y = vect.Y + 30;
                    spriteBatch.DrawString(_font, playerO.Score.ToString(), vect, Color.Black);
    
                    break;

                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                // the windows is either in full screen mode or 3/4 screen snap mode sp
                // the game is playable, display the board and the game state
                // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                case WindowState.Full:
                case WindowState.Snap3Quarter:
                    // draw the backgroeund 
                    background.Draw(spriteBatch, GraphicsDevice);

                    // draw the state of the board and the player scores
                    board.Draw(spriteBatch);
                    playerO.Draw(spriteBatch, 0.83);
                    playerX.Draw(spriteBatch, 0.05);

                    if (GameState._gameOver == true)
                    {
                        // draw line threw winning tiles
                        board.DrawWinLine(Content, spriteBatch, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

                        // draw the new game tile
                        newGameTile.Draw(spriteBatch);

                        // check to see if the new game tile is clicked
                        if (newGameTile.ButtonClicked)
                        {
                            // reset the game board for a new game
                            ResetGame();
                        }
                    }
                    break;
            }

            // Stop drawing
            spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
