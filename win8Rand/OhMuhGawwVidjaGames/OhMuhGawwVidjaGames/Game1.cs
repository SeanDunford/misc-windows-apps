using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

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


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
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
           SpriteFont sF = Content.Load<SpriteFont>("font"); 
           _score = new Score(sF, gameBoundaries );

           //Aggregate reference to all game objects
            gameObjects = new GameObjects { PlayerPaddle = playerPaddle, ComputerPaddle = computerPaddle, Ball = _ball ,Score = _score}; 

            //Play Music
            //MediaPlayer.Play(Content.Load<Song>("Music"));
            
        }
        protected override void UnloadContent()
        {
            
        }
        //Runs very fast used to update locations and gamestate
        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit(); 
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                gameObjects.Score.PlayerScore = 0;
                gameObjects.Score.ComputerScore = 0; 
                gameObjects.Score.gameOver = false; 
                _ball.Location = Vector2.Zero; 
                _ball.AttachTo(playerPaddle);
                
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
            GraphicsDevice.Clear(Color.Turquoise);
            _spriteBatch.Begin();
            //Must Begin
            playerPaddle.Draw(_spriteBatch);
            _ball.Draw(_spriteBatch);
            computerPaddle.Draw(_spriteBatch);
            _score.Draw(_spriteBatch);
            if (gameObjects.Score.gameOver == false)
            {    

            }
            else if (gameObjects.Score.didThePlayerWin == true)
            {
                gameObjects.Score.PlayerWin(_spriteBatch);
            }
            else
            {
                gameObjects.Score.ComputerWin(_spriteBatch);
            }
            //Must End
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
