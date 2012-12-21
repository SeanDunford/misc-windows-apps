using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; 

namespace OhMuhGawwVidjaGames
{
    public enum PlayerTypes
    {
        Human, Computer
    }
    public class Paddle : Sprite
    {
        private readonly PlayerTypes playerType;

        public Paddle(Texture2D texture, Vector2 location, Rectangle screenBounds, PlayerTypes playerType): base(texture, location, screenBounds)
        {
            this.playerType = playerType; 
        }
        public override void Update(GameTime gameTime, GameObjects gameObjects)
        {
            //if (gameObjects.Score.gameOver == false)
           // {
                if (playerType == PlayerTypes.Computer)
                {
                    //AI ...no robin williams
                    var random = new Random();
                    var reactionThreshold = random.Next(0, 25);


                    if ((gameObjects.Ball.Location.Y + gameObjects.Ball.Height / 2) == (Location.Y + Height / 2))
                    {
                        Velocity = Vector2.Zero;
                    }
                    else if (gameObjects.Ball.Location.Y + gameObjects.Ball.Height < Location.Y + reactionThreshold)
                    {
                        Velocity = new Vector2(0, -3f);
                    }
                    else if (gameObjects.Ball.Location.Y > Location.Y + Height + reactionThreshold)
                    {
                        Velocity = new Vector2(0, 3f);
                    }
                }
                if (playerType == PlayerTypes.Human)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        var newVelocity = new Vector2(0, -3f);
                        Velocity = newVelocity;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        var newVelocity = new Vector2(0, 3f);
                        Velocity = newVelocity;
                    }
                    else if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        var newVelocity = Vector2.Zero;
                        Velocity = newVelocity;
                    }
                    else if ((Location.Y == 0 || Location.Y == (gameBoundaries.Height - _texture.Height)))
                    {
                        if (Velocity != Vector2.Zero)
                        {
                            var newVelocity = Vector2.Zero;
                            Velocity = newVelocity;
                        }
                    }
                    //Zero Velocity if clipping screen 
                }
           // }
            base.Update(gameTime, gameObjects);
        }
        protected override void CheckBounds()
        {
            Location.Y = MathHelper.Clamp(Location.Y, 0, (gameBoundaries.Height - _texture.Height));
        }
    }   
}