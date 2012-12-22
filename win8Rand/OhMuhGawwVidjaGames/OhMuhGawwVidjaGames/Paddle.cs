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

namespace OhMuhGawwVidjaGames
{
    public enum PlayerTypes
    {
        Human, Computer
    }
    public class Paddle : Sprite
    {
        private readonly PlayerTypes playerType;
        private Boolean mouseMove = false;
        private Vector2 touchPosition = Vector2.Zero;



        public Paddle(Texture2D texture, Vector2 location, Rectangle screenBounds, PlayerTypes playerType): base(texture, location, screenBounds)
        {
            
            this.playerType = playerType; 
        }
        public override void Update(GameTime gameTime, GameObjects gameObjects)
        {
                if (playerType == PlayerTypes.Computer)
                {
                    //AI ...no robin williams
                    var random = new Random();
                    var reactionThreshold = random.Next(10, 50);//edit for computer difficulty


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
                    if ((Location.Y == 0 || Location.Y == (gameBoundaries.Height - _texture.Height)))
                    {
                        if (Velocity != Vector2.Zero)
                        {
                            var newVelocity = Vector2.Zero;
                            Velocity = newVelocity;
                            mouseMove = false;
                        }
                    }
                    var touchPoint = TouchPanel.GetState().FirstOrDefault();
                    if (touchPoint.State != TouchLocationState.Invalid)
                    {
                        mouseMove = true;
                        touchPosition = new Vector2(0f, touchPoint.Position.Y);
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {   // Add touch 
                        //Mouse overrides keyboard input
                        //Mouse and Touch shoudl be handled as same event sort of so they don't compete 
                        mouseMove = true;
                        touchPosition = new Vector2(0f, Mouse.GetState().Y);
                    }
                    if (mouseMove)
                    {
                        if ((touchPosition.Y < (Location.Y + Height / 2) + 10f) && (touchPosition.Y > (Location.Y + Height / 2) - 10f))
                        {
                            mouseMove = false;
                            Velocity = Vector2.Zero;

                        }
                        else if (touchPosition.Y > Location.Y + Height / 2)
                        {
                            var newVelocity = new Vector2(0, 3f);
                            Velocity = newVelocity;
                        }
                        else if (touchPosition.Y < Location.Y + Height / 2)
                        {
                            var newVelocity = new Vector2(0, -3f);
                            Velocity = newVelocity;
                        }
                    }

                    else if (Keyboard.GetState().IsKeyDown(Keys.Left))
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
                }
                base.Update(gameTime, gameObjects);
                }        
        protected override void CheckBounds()
        {
            Location.Y = MathHelper.Clamp(Location.Y, 0, (gameBoundaries.Height - _texture.Height));
        }
    }   
}