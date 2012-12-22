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

namespace EggScramble
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bricksTexture;
        Texture2D groundTexture;
        Texture2D skyTexture;
        Texture2D eggTexture;
        Dog dog;
        Chicken chicken;
        Vector2 eggSpeed = new Vector2(0, 150);
        FormattedValue score = new FormattedValue("SCORE: {0}");
        FormattedValue level = new FormattedValue("LEVEL: {0}");
        FormattedValue dropsLeft = new FormattedValue("MISSES LEFT: {0}");
        FormattedValue catchesLeft = new FormattedValue("GOAL: {0}");
        SpriteFont scoreFont;
        SpriteFont gameOverFont;
        bool gameOver = true;
        bool pressed = false;
        ScaledSpriteBatch scaledSpriteBatch;


        Random rand;

        List<Egg> eggs = new List<Egg>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

#if !NETFX_CORE
            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif

            dog = new Dog();
            dog.DropEgg += dog_DropEgg;
            dog.Position = new Vector2(400 - dog.Center.X, 50);
            chicken = new Chicken();
            chicken.Position = new Vector2(400 - chicken.Center.X, 330);
            rand = new Random();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        void dog_DropEgg(object sender, EventArgs e)
        {
            eggs.Add(new Egg(new Vector2(dog.Position.X + (dog.Size.X / 2) - (Egg.EggSize.X / 2), dog.Position.Y + 90), eggTexture, eggSpeed));
        }

        private void NewGame()
        {
            gameOver = false;
            level.Value = 0;
            score.Value = 0;
            LevelUp();
            chicken.Position.X = 400 - chicken.Size.X / 2;
            dog.Position.X = 400 - dog.Size.X / 2;
            dog.Start();
        }

        private void LevelUp()
        {
            level.Value++;
            dog.Speed = 100 + level.Value * 25;
            chicken.Speed = dog.Speed;
            dog.DropDelay = Math.Max(1 / (level.Value + 1), .1);
            dropsLeft.Value = 5;
            catchesLeft.Value = 10;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scaledSpriteBatch = new ScaledSpriteBatch(spriteBatch);
#if NETFX_CORE
            // this next statement allows us to specify a larger font size for Win8 MonoGame. Because
            // XNA and MonoGame use bitmapped fonts, if we didn't scale this then when we went to a bigger screen 
            // for Win8 the text would appear blurry and antialiased. Ideally you could specify even more sizes and scales to look
            // good at even higher resolutions.
            // In this case we decided to specify font sizes 3 times bigger for Win8, so we need to set TextScale to the inverse of that.
            scaledSpriteBatch.TextScale = 1 / 3f;
#endif
            bricksTexture = Content.Load<Texture2D>("bricks");
            dog.Texture = Content.Load<Texture2D>("dog");
            chicken.Texture = Content.Load<Texture2D>("chicken");
            groundTexture = Content.Load<Texture2D>("ground");
            skyTexture = Content.Load<Texture2D>("sky"); 
            eggTexture = Content.Load<Texture2D>("egg");
            scoreFont = Content.Load<SpriteFont>("scoreFont");
            gameOverFont = Content.Load<SpriteFont>("gameOverFont");
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            UpdateScaleAndOffset();
            bool isTouching = false;
            Vector2 touchPosition = Vector2.Zero;
            
            var touchPoint = TouchPanel.GetState().FirstOrDefault();
            if (touchPoint.State != TouchLocationState.Invalid)
            {
                isTouching = true;
                touchPosition = (touchPoint.Position - scaledSpriteBatch.Offset) / scaledSpriteBatch.GlobalScale;
            }
            else
            {
                isTouching = false;
#if NETFX_CORE
                var mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    isTouching=true;
                    touchPosition = (new Vector2(mouseState.X, mouseState.Y) - scaledSpriteBatch.Offset) / scaledSpriteBatch.GlobalScale;
                }
#endif
            }
            chicken.Velocity = Vector2.Zero;
            
            if (isTouching)
            {
                pressed = true;
                if (!gameOver)
                {
                    Vector2 v = new Vector2(touchPosition.X - chicken.Center.X, 0);
                    if (v.Length() > 10)
                    {
                        v.Normalize();
                        v *= chicken.Speed;
                        chicken.Velocity = v;
                    }
                }
            }
            else
            {
                if (pressed && gameOver)
                {
                    NewGame();
                    pressed = false;
                }
                else
                {
                    pressed = false;
                }
            }

            if (!gameOver)
            {
                double elapsed = gameTime.ElapsedGameTime.TotalSeconds;
                dog.Update(elapsed);
                chicken.Update(elapsed); 
                for (int i = eggs.Count - 1; i >= 0; i--)
                {
                    bool collidesWithChicken = false;
                    eggs[i].Update(elapsed, chicken, out collidesWithChicken);
                    if (collidesWithChicken)
                    {
                        score.Value += 100;
                        catchesLeft.Value--;
                        if (catchesLeft.Value == 0) LevelUp();
                        eggs.RemoveAt(i);
                    }
                    else if (eggs[i].Position.Y > chicken.Position.Y + 90)
                    {
                        dropsLeft.Value--;
                        if (dropsLeft.Value == 0)
                        {
                            GameOver();
                            return;
                        }
                        eggs.RemoveAt(i);
                    }
                }
            }
            base.Update(gameTime);
        }

        private void UpdateScaleAndOffset()
        {
#if NETFX_CORE
            var scaleY = GraphicsDevice.Viewport.Height / 480f;
            var scaleX = GraphicsDevice.Viewport.Width / 800f;
            scaledSpriteBatch.GlobalScale = Math.Min(scaleX, scaleY);
            scaledSpriteBatch.Offset = new Vector2((int)((GraphicsDevice.Viewport.Width - (800 * scaledSpriteBatch.GlobalScale)) / 2), (int)((GraphicsDevice.Viewport.Height - (480 * scaledSpriteBatch.GlobalScale)) / 2));
#else
            scaledSpriteBatch.GlobalScale = 1;
            scaledSpriteBatch.Offset = Vector2.Zero;
#endif
        }

        private void DrawBackground()
        {
            scaledSpriteBatch.Draw(groundTexture, new Vector2(0, 402), new Vector2(800, 128));
            scaledSpriteBatch.Draw(skyTexture, new Vector2(0, 0), new Vector2(800, 152));
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    scaledSpriteBatch.Draw(bricksTexture, new Vector2(i * 50, (j * 50 + 152)), new Vector2(51,51));
                }
            }
        }

        private void GameOver()
        {
            eggs.Clear();
            gameOver = true;
        }

        private IEnumerable<Sprite> GameSprites
        {
            get
            {
                yield return dog;
                yield return chicken;
                foreach (var egg in eggs) yield return egg;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            scaledSpriteBatch.Begin();
            DrawBackground();
            Vector2 drawPos = new Vector2(20, 10);
            float dy = scaledSpriteBatch.MeasureString(scoreFont, score.Text).Y;
            scaledSpriteBatch.DrawString(scoreFont, score.Text, drawPos, Color.Black);
            drawPos.Y += dy;
            scaledSpriteBatch.DrawString(scoreFont, level.Text, drawPos, Color.Black);
            drawPos.Y += dy;
            scaledSpriteBatch.DrawString(scoreFont, dropsLeft.Text, drawPos, Color.Black); 
            drawPos.Y += dy;
            scaledSpriteBatch.DrawString(scoreFont, catchesLeft.Text, drawPos, Color.Black);
            
            foreach (var sprite in GameSprites)
            {
                sprite.Draw(scaledSpriteBatch);
            }

            if (gameOver)
            {
                Vector2 textSize = scaledSpriteBatch.MeasureString(gameOverFont, "TAP FOR NEW GAME");
                Vector2 pos = ((new Vector2(800, 400) / 2) - (textSize / 2));
                scaledSpriteBatch.DrawString(gameOverFont, "TAP FOR NEW GAME", pos + new Vector2(4, 4), new Color(0, 0, 0, 128));
                scaledSpriteBatch.DrawString(gameOverFont, "TAP FOR NEW GAME", pos, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
