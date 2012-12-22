using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Windows.UI.Core;
using Windows.UI.ViewManagement; 

//http://stackoverflow.com/questions/5041457/cannot-load-a-spritefont-in-xna4


namespace OhMuhGawwVidjaGames
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameObjects gameObjects; 
        private Paddle playerPaddle;
        private Paddle computerPaddle; 
        private Ball _ball;
        private Score _score; 
        private Rectangle gameBoundaries;
        private SpriteFont sF;
        private Sprite Icon;
        private Texture2D IconTexture; 
        private Boolean windowIsSnapped;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }
        protected override void Initialize() //non Graphic initialization needed before game loads
        {
            IsMouseVisible = true;   
            base.Initialize();
        }
        protected override void LoadContent() //Load all Game Contenet
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            gameBoundaries = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            

            //Paddle creation
            var paddleTexture = Content.Load<Texture2D>("paddle");
            var personPaddleLocation = new Vector2(10f, gameBoundaries.Height / 2);
            var computerPaddleLocation = new Vector2(gameBoundaries.Width - paddleTexture.Width - 10, gameBoundaries.Height/2);
            playerPaddle = new Paddle(paddleTexture, personPaddleLocation, gameBoundaries, PlayerTypes.Human );
            computerPaddle = new Paddle(paddleTexture, computerPaddleLocation, gameBoundaries, PlayerTypes.Computer);
            //Ball creation
            _ball = new Ball(Content.Load<Texture2D>("Ball"), Vector2.Zero, gameBoundaries);
            _ball.AttachTo(playerPaddle);

           // Score Creation
            sF = Content.Load<SpriteFont>("font");
            _score = new Score(sF, gameBoundaries );

            // Icon 
            IconTexture = new Texture2D(GraphicsDevice, 100, 100); 
            IconTexture = Content.Load<Texture2D>("logo150150.png");
            Icon = new Icon(IconTexture, Vector2.Zero, gameBoundaries);

           //Aggregate reference to all game objects
            gameObjects = new GameObjects { PlayerPaddle = playerPaddle, ComputerPaddle = computerPaddle, Ball = _ball ,Score = _score}; 

            //Play Music
            //MediaPlayer.Play(Content.Load<Song>("Music"));
            
        }
        protected override void UnloadContent()
        {
            
        }
        private Boolean checkGameWindow(){
            Boolean lSnapped = false; 
            if (ApplicationView.Value == ApplicationViewState.Snapped)
            {
                lSnapped = true;
                gameBoundaries = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
            else if (ApplicationView.Value == ApplicationViewState.Filled)
            {
                lSnapped = true;
                gameBoundaries = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
            else if (ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
            {
                lSnapped = false;
                gameBoundaries = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
            }
            return lSnapped; 
         }
        //Runs very fast used to update locations and gamestate
        protected override void Update(GameTime gameTime)
        {
            windowIsSnapped = checkGameWindow();
             if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit(); 
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R) || ((gameObjects.Score.gameOver == true ||gameObjects.Ball.showTutorial ) && (Mouse.GetState().LeftButton == ButtonState.Pressed || TouchPanel.GetState().FirstOrDefault().State != TouchLocationState.Invalid)))
            {
              
                gameObjects.Score.PlayerScore = 0;
                gameObjects.Score.ComputerScore = 0; 
                gameObjects.Score.gameOver = false; 
                _ball.Location = Vector2.Zero; 
                _ball.AttachTo(playerPaddle);
                _ball.launchBall(); 
                
            }
            else if (gameObjects.Score.PlayerScore > 5 && gameObjects.Score.gameOver == false)
            {
                
                gameObjects.Score.didThePlayerWin = true; 
                gameObjects.Score.gameOver = true; 
            }
            else if (gameObjects.Score.ComputerScore > 5 && gameObjects.Score.gameOver == false)
            {
               
                gameObjects.Score.didThePlayerWin = false; 
                gameObjects.Score.gameOver = true; 
            }
            else if (gameObjects.Score.gameOver == false)
            {
                playerPaddle.Update(gameTime, gameObjects);
                computerPaddle.Update(gameTime, gameObjects);
                _ball.Update(gameTime, gameObjects);
                _score.Update(gameTime, gameObjects);
            }
            base.Update(gameTime);      
        }
        //Draw graphics on the screen
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(76, 246, 255));
            _spriteBatch.Begin();
            //Must Begin
            if (windowIsSnapped)
            {
                var IconLocation = new Vector2((gameBoundaries.Width - Icon.Width)/2, (gameBoundaries.Height - Icon.Height)/2);
                Icon.Location = IconLocation; 
                Icon.Draw(_spriteBatch); 
            }
            else{
                playerPaddle.Draw(_spriteBatch);
                _ball.Draw(_spriteBatch);
                computerPaddle.Draw(_spriteBatch);
                _score.Draw(_spriteBatch);
                if (gameObjects.Ball.showTutorial)
                {
                    var Begin1 = "Use touch, mouse, or left right keys to move";
                    var xPositionBegin1 = ((gameBoundaries.Width / 2) - (sF.MeasureString(Begin1).X / 2));
                    var PositionBegin1 = new Vector2(xPositionBegin1, gameBoundaries.Height / 2 - 200);

                    var Begin2 = "Score points by getting the ball past the computer";
                    var xPositionBegin2 = ((gameBoundaries.Width / 2) - (sF.MeasureString(Begin2).X / 2));
                    var PositionBegin2 = new Vector2(xPositionBegin2, gameBoundaries.Height / 2 - 100);

                    var Begin3 = "First to 6 wins!";
                    var xPositionBegin3 = ((gameBoundaries.Width / 2) - (sF.MeasureString(Begin3).X / 2));
                    var PositionBegin3 = new Vector2(xPositionBegin3, gameBoundaries.Height / 2);

                    var Begin4 = "Press Space or Right Click to reset";
                    var xPositionBegin4 = ((gameBoundaries.Width / 2) - (sF.MeasureString(Begin4).X / 2));
                    var PositionBegin4 = new Vector2(xPositionBegin4, gameBoundaries.Height / 2 + 100);

                    _spriteBatch.DrawString(sF, Begin1, PositionBegin1, Color.Purple);
                    _spriteBatch.DrawString(sF, Begin2, PositionBegin2, Color.Purple);
                    _spriteBatch.DrawString(sF, Begin3, PositionBegin3, Color.Purple);
                    _spriteBatch.DrawString(sF, Begin4, PositionBegin4, Color.Purple);

                }
                if (!gameObjects.Score.gameOver)
                { }
                else if (gameObjects.Score.didThePlayerWin == true)
                {
                    gameObjects.Score.PlayerWin(_spriteBatch);
                }
                else
                {
                    gameObjects.Score.ComputerWin(_spriteBatch);
                }
            }
            //Must End
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
