using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI.Screens
{
    class GameScreen : Screen
    {
        private readonly Texture2D arenaTexture;
        private readonly Rectangle arenaRect;
        private readonly int width;

        public SpriteAnimation FirstSprite { get; set; }
        public HealthBar FirstHealthBar { get; set; }
        private PlayerState firstState;

        public SpriteAnimation SecondSprite { get; set; }
        public HealthBar SecondHealthBar { get; set; }
        private PlayerState secondState;

        public GameScreen(Game game, int width, Texture2D arenaTexture, Rectangle arenaRect) : base(game)
        {
            this.width = width;
            this.arenaTexture = arenaTexture;
            this.arenaRect = arenaRect;
        }


        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            firstState = FirstSprite.HandleSpriteMovement(gameTime);
            secondState = SecondSprite.HandleSpriteMovement(gameTime);

            if (firstState == PlayerState.Move || secondState == PlayerState.Move)
                CollideDetector.RepairMoveCollision(FirstSprite, SecondSprite, width);
            CollideDetector.HitCollision(FirstSprite, SecondSprite, gameTime);
            FirstHealthBar.Update(FirstSprite.Information.Health);
            SecondHealthBar.Update(SecondSprite.Information.Health);
            base.Update(gameTime);
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
            base.Draw(gameTime);
        }
    }
}
