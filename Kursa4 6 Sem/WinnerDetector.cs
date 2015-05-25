using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MortalKombatXI
{
    class WinnerDetector
    {
        public static void DetectWinner(GameTime gameTime, SpriteAnimation first, SpriteAnimation second)
        {
            SpriteAnimation winner = null;
            bool hasWinner = false;
            if (first.Information.Health == 0)
            {
                winner = second;
                hasWinner = true;
                GameState.SecondWin();
            }
            if (second.Information.Health == 0)
            {
                winner = first;
                hasWinner = true;
                GameState.FirstWin();
            }
            if (hasWinner)
            {
                if (!GameState.NextRound(first, second))
                {
                    GameState.IsEnd = true;
                    winner.Win(gameTime);
                }
            }
        }

    }
}
