using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI.Screens
{
    class GameScreen : Screen
    {
        private readonly Texture2D arenaTexture;
        private readonly Rectangle arenaRect;
        private readonly int width;
        private readonly SpriteFont font;
        private readonly SpriteFont secondaryFont;

        public SpriteAnimation FirstSprite { get; set; }
        public HealthBar FirstHealthBar { get; set; }
        private PlayerState firstState;

        public SpriteAnimation SecondSprite { get; set; }
        public HealthBar SecondHealthBar { get; set; }
        private PlayerState secondState;


        public GameScreen(Game game, SpriteFont font, SpriteFont secondaryFont, int width, Texture2D arenaTexture, Rectangle arenaRect) : base(game)
        {
            this.font = font;
            this.secondaryFont = secondaryFont;
            this.width = width;
            this.arenaTexture = arenaTexture;
            this.arenaRect = arenaRect;
        }

        public override void Update(GameTime gameTime)
        {
            if (!GameState.IsPaused)
            {
                if (GameState.IsEnd)
                {
                    ShowLabels.ShowEndGame();
                    FirstSprite.HandleSpriteMovement(gameTime);
                    SecondSprite.HandleSpriteMovement(gameTime);
                    if (!GameState.IsEnd)
                    {
                        FirstSprite.Stop();
                        SecondSprite.Stop();
                    }
                }
                else
                {
                    ShowLabels.ShowWinner();
                    ShowLabels.ShowRound();
                    firstState = FirstSprite.HandleSpriteMovement(gameTime);
                    secondState = SecondSprite.HandleSpriteMovement(gameTime);

                    if (firstState == PlayerState.Move || secondState == PlayerState.Move)
                        CollideDetector.RepairMoveCollision(FirstSprite, SecondSprite, width);
                    CollideDetector.HitCollision(FirstSprite, SecondSprite, gameTime);
                    FirstHealthBar.Update(FirstSprite.Information.Health);
                    SecondHealthBar.Update(SecondSprite.Information.Health);
                    WinnerDetector.DetectWinner(gameTime, FirstSprite, SecondSprite);
                }
                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //arena
            ScreenSpriteBatch.Draw(
                arenaTexture,
                arenaRect,
                Color.White);
            //first player
            ScreenSpriteBatch.Draw(
                FirstSprite.CurrentTexture,
                FirstSprite.Position,
                FirstSprite.SourceRect,
                Color.White,
                0f,
                FirstSprite.Origin,
                1.0f,
                SpriteEffects.None,
                0);
            //second player
            ScreenSpriteBatch.Draw(
                SecondSprite.CurrentTexture,
                SecondSprite.Position,
                SecondSprite.SourceRect,
                Color.White,
                0f,
                SecondSprite.Origin,
                1.0f,
                SpriteEffects.FlipHorizontally,
                0);
            //health first
            ScreenSpriteBatch.Draw(
                FirstHealthBar.Texture,
                FirstHealthBar.Position,
                FirstHealthBar.SourceRect,
                Color.White,
                0f,
                FirstHealthBar.Origin,
                1.0f,
                SpriteEffects.None,
                0);
            //health second
            ScreenSpriteBatch.Draw(
                SecondHealthBar.Texture,
                SecondHealthBar.Position,
                SecondHealthBar.SourceRect,
                Color.White,
                0f,
                SecondHealthBar.Origin,
                1.0f,
                SpriteEffects.None,
                0);
            //name and win round first
            ScreenSpriteBatch.DrawString(
                secondaryFont,
                GameSettings.firstName + " 0" + GameState.firstWin,
                GameSettings.NameWinBar,
                Color.White,
                0f,
                secondaryFont.MeasureString(GameSettings.firstMeasure),
                1f,
                SpriteEffects.None, 
                0.5f
                );
            ScreenSpriteBatch.DrawString(
                secondaryFont,
                GameSettings.secondName + " 0" + GameState.secondWin,
                new Vector2(width - GameSettings.NameWinBar.X, GameSettings.NameWinBar.Y), 
                Color.White,
                0f,
                secondaryFont.MeasureString(GameSettings.secondMeasure),
                1f,
                SpriteEffects.None,
                0.5f
                );
            //name and win round second
            if (GameState.ShowRound)
                ScreenSpriteBatch.DrawString(
                    font,
                    GameState.CurrentRound + " Round!",
                    new Vector2(width / 2 - 100, 300),
                    Color.White
                    );
            if (GameState.ShowWinner)
                ScreenSpriteBatch.DrawString(
                    font,
                    GameState.CurrentWinner + " win the round!",
                    new Vector2(width / 2 - 200, 300),
                    Color.White
                    );
            if (GameState.IsEnd)
                ScreenSpriteBatch.DrawString(
                    font,
                    GameState.CurrentWinner + " win the game!",
                    new Vector2(width / 2 - 200, 300),
                    Color.White
                    );
            base.Draw(gameTime);
        }
    }
}
