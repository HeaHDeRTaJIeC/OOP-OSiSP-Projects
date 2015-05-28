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
        private readonly Color color = Color.DarkGray;
        private readonly Color colorAdditional = Color.DarkRed;
        private readonly Texture2D background;
        private readonly Rectangle backgroundRect;
        private readonly SpriteFont fontMoves;
        private readonly SpriteFont fontPlayers;
        private Vector2 startPosition = new Vector2(100, 100);
        private const int Delta = 70;

        private readonly String[] movesName =
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

        private readonly String[] first;
        private readonly String[] second;
        

        public HelpScreen(Game game, Texture2D background, Rectangle backgroundRect, SpriteFont fontMoves, SpriteFont fontPlayers,  PlayerControls firstPlayer, PlayerControls secondPlayer) : base(game)
        {
            this.background = background;
            this.backgroundRect = backgroundRect;
            this.fontMoves = fontMoves;
            this.fontPlayers = fontPlayers;
            first = new String[9];
            first[0] = movesName[0] + firstPlayer.MoveLeft;
            first[1] = movesName[1] + firstPlayer.MoveRight;
            first[2] = movesName[2] + firstPlayer.MoveUp;
            first[3] = movesName[3] + firstPlayer.MoveDown;
            first[4] = movesName[4] + firstPlayer.HandHitLeft;
            first[5] = movesName[5] + firstPlayer.HandHitRight;
            first[6] = movesName[6] + firstPlayer.UppercodeHit;
            first[7] = movesName[7] + firstPlayer.LegHit;
            first[8] = movesName[8] + firstPlayer.Block;
            second = new String[9];
            second[0] = movesName[0] + secondPlayer.MoveLeft;
            second[1] = movesName[1] + secondPlayer.MoveRight;
            second[2] = movesName[2] + secondPlayer.MoveUp;
            second[3] = movesName[3] + secondPlayer.MoveDown;
            second[4] = movesName[4] + secondPlayer.HandHitLeft;
            second[5] = movesName[5] + secondPlayer.HandHitRight;
            second[6] = movesName[6] + secondPlayer.UppercodeHit;
            second[7] = movesName[7] + secondPlayer.LegHit;
            second[8] = movesName[8] + secondPlayer.Block;

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
                    new Vector2(startPosition.X, startPosition.Y + i * Delta),
                    color);
            for (int i = 0; i < 9; i++)
                ScreenSpriteBatch.DrawString(
                    fontMoves,
                    second[i],
                    new Vector2(startPosition.X + 600, startPosition.Y + i * Delta),
                    color);
            base.Draw(gameTime);
        }
    }
}
