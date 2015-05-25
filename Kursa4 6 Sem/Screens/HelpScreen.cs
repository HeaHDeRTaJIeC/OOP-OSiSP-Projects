using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MortalKombatXI.Screens
{
    internal class HelpScreen : Screen
    {
        private Color color = Color.DarkGray;
        private Color colorAdditional = Color.DarkRed;
        private Texture2D background;
        private Rectangle backgroundRect;
        private SpriteFont fontMoves;
        private SpriteFont fontPlayers;
        private Vector2 startPosition = new Vector2(100, 100);
        private int delta = 70;
        private String[] movesName =
        {
            "Move left        ",
            "Move right       ",
            "Stand up         ",
            "Sit down         ",
            "Left hand kick   ",
            "Right hand kick  ",
            "Uppercode        ",
            "Leg kick         ",
            "Block            "
        };

        private String[] first;
        private String[] second;
        

        public HelpScreen(Game game, Texture2D background, Rectangle backgroundRect, SpriteFont fontMoves, SpriteFont fontPlayers,  PlayerControls firstPlayer, PlayerControls secondPlayer) : base(game)
        {
            this.background = background;
            this.backgroundRect = backgroundRect;
            this.fontMoves = fontMoves;
            this.fontPlayers = fontPlayers;
            first = new String[9];
            first[0] = movesName[0] + firstPlayer.MoveLeft.ToString();
            first[1] = movesName[1] + firstPlayer.MoveRight.ToString();
            first[2] = movesName[2] + firstPlayer.MoveUp.ToString();
            first[3] = movesName[3] + firstPlayer.MoveDown.ToString();
            first[4] = movesName[4] + firstPlayer.HandHitLeft.ToString();
            first[5] = movesName[5] + firstPlayer.HandHitRight.ToString();
            first[6] = movesName[6] + firstPlayer.UppercodeHit.ToString();
            first[7] = movesName[7] + firstPlayer.LegHit.ToString();
            first[8] = movesName[8] + firstPlayer.Block.ToString();
            second = new String[9];
            second[0] = movesName[0] + secondPlayer.MoveLeft.ToString();
            second[1] = movesName[1] + secondPlayer.MoveRight.ToString();
            second[2] = movesName[2] + secondPlayer.MoveUp.ToString();
            second[3] = movesName[3] + secondPlayer.MoveDown.ToString();
            second[4] = movesName[4] + secondPlayer.HandHitLeft.ToString();
            second[5] = movesName[5] + secondPlayer.HandHitRight.ToString();
            second[6] = movesName[6] + secondPlayer.UppercodeHit.ToString();
            second[7] = movesName[7] + secondPlayer.LegHit.ToString();
            second[8] = movesName[8] + secondPlayer.Block.ToString();

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenSpriteBatch.Draw(
                background,
                backgroundRect,
                Color.White
                );
            ScreenSpriteBatch.DrawString(
                fontPlayers,
                "First player",
                new Vector2(startPosition.X, startPosition.Y - 100),
                colorAdditional);
            ScreenSpriteBatch.DrawString(
                fontPlayers,
                "Second player",
                new Vector2(startPosition.X + 600, startPosition.Y - 100),
                colorAdditional);
            for (int i = 0 ; i < 9; i++)
                ScreenSpriteBatch.DrawString(
                    fontMoves,
                    first[i],
                    new Vector2(startPosition.X, startPosition.Y + i * delta),
                    color);
            for (int i = 0; i < 9; i++)
                ScreenSpriteBatch.DrawString(
                    fontMoves,
                    second[i],
                    new Vector2(startPosition.X + 600, startPosition.Y + i * delta),
                    color);
            base.Draw(gameTime);
        }
    }
}
