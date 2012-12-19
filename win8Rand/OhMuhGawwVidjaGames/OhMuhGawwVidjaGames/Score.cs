

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace OhMuhGawwVidjaGames
{
    public class Score
    {
        private readonly SpriteFont font; 
        private readonly Rectangle gameBoundaries;

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
    }
}
