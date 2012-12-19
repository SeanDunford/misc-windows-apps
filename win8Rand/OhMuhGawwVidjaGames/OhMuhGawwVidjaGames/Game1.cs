using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OhMuhGawwVidjaGames
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {

        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private GameObjects gameObjects; 
        private Paddle playerPaddle;
        private Paddle computerPaddle; 
        private Ball _ball;
        private Score score; 

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
           //  var sF = Content.Load<SpriteFont>("SpriteFont1"); 
           //score = new Score(sF, gameBoundaries ); 

            gameObjects = new GameObjects { PlayerPaddle = playerPaddle, ComputerPaddle = computerPaddle, Ball = _ball ,Score = score}; 
            //Aggregate reference to all game objects
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
                _ball.Location = Vector2.Zero; 
                _ball.AttachTo(playerPaddle);
            }

            playerPaddle.Update(gameTime, gameObjects);
            computerPaddle.Update(gameTime, gameObjects);
            _ball.Update(gameTime, gameObjects);

            base.Update(gameTime);
        }
        //Draw graphocs on the screen
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            _spriteBatch.Begin();
            //Must Begin
            playerPaddle.Draw(_spriteBatch);
            _ball.Draw(_spriteBatch);
            computerPaddle.Draw(_spriteBatch);
           //Finter score.Draw(_spriteBatch); 
            _spriteBatch.End();
            //Must End
            base.Draw(gameTime);
        }
    }
}
