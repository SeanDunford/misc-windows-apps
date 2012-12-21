

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace OhMuhGawwVidjaGames
{
    public class Score
    {
        private readonly SpriteFont font; 
        private readonly Rectangle gameBoundaries;
        public Boolean gameOver = false;
        public Boolean didThePlayerWin = false; 

        public int PlayerScore { get; set; }
        public int ComputerScore { get; set; }

        public Score(SpriteFont font, Rectangle gameBoundaries){
            this.font = font; 
            this.gameBoundaries = gameBoundaries; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var scoreText = string.Format("{0}:{1}", PlayerScore, ComputerScore ); 
            var xPosition = ((gameBoundaries.Width/2 )- (font.MeasureString(scoreText).X/2));
            var Position = new Vector2(xPosition, gameBoundaries.Height - 100);

            spriteBatch.DrawString(font, scoreText, Position, Color.White); 
        }
        public void Update(GameTime gameTime, GameObjects gameObjects)
        {
            if (gameObjects.Ball.Location.X + gameObjects.Ball.Width < 0)
            {
                ComputerScore++;
                gameObjects.Ball.AttachTo(gameObjects.PlayerPaddle);
            } 
            else if (gameObjects.Ball.Location.X > gameBoundaries.Width)
            {
                PlayerScore++;
                gameObjects.Ball.AttachTo(gameObjects.PlayerPaddle);
            }
        }
        public void PlayerWin(SpriteBatch spriteBatch)
        {
            var YouWon = "OhMUHGawww!!!!1!!!! You Won!";
            var restart = "Press R to (R)estart"; 
            var xPositionYouWon = ((gameBoundaries.Width / 2) - (font.MeasureString(YouWon).X / 2));
            var xPosition2Restart = ((gameBoundaries.Width / 2) - (font.MeasureString(restart).X / 2));
            var PositionYouWon = new Vector2(xPositionYouWon, gameBoundaries.Height / 2);
            var PositionRestart = new Vector2(xPosition2Restart, (gameBoundaries.Height / 2) + 200);

            spriteBatch.DrawString(font, YouWon, PositionYouWon, Color.Purple);
            spriteBatch.DrawString(font, restart, PositionRestart, Color.Purple);
        }
        public void ComputerWin(SpriteBatch spriteBatch)
        {
            var Sorry = "Sorry Bruh :( you lost";
            var restart = "Press R to (R)estart";
            var xPositionYouWon = ((gameBoundaries.Width / 2) - (font.MeasureString(Sorry).X / 2));
            var xPosition2Restart = ((gameBoundaries.Width / 2) - (font.MeasureString(restart).X / 2));
            var PositionYouWon = new Vector2(xPositionYouWon, gameBoundaries.Height / 2);
            var PositionRestart = new Vector2(xPosition2Restart, (gameBoundaries.Height / 2) + 200);

            spriteBatch.DrawString(font, Sorry, PositionYouWon, Color.Purple);
            spriteBatch.DrawString(font, restart, PositionRestart, Color.Purple);
        }
    }
}
