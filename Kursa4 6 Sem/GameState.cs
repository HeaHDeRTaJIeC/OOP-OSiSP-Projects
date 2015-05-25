using System;
using Microsoft.Xna.Framework;

namespace MortalKombatXI
{
    class GameState
    {
        public static int CurrentRound { get; private set; }
        public static String CurrentWinner { get; private set; }
        public static int firstWin { get; private set; }
        public static int secondWin { get; private set; }
        public static bool ShowRound { get; set; }
        public static bool ShowWinner { get; set; }
        public static bool ShowMenu { get; set; }
        public static bool IsPaused { get; set; }
        public static bool IsEnd { get; set; }

        public static void StartNewGame()
        {
            firstWin = 0;
            secondWin = 0;
            CurrentRound = 1;
            CurrentWinner = "None";
            ShowWinner = false;
            ShowRound = true;
        }

        public static void FirstWin()
        {
            CurrentWinner = GameSettings.firstName;
            firstWin++;
        }

        public static void SecondWin()
        {
            CurrentWinner = GameSettings.secondName;
            secondWin++;
        }

        public static bool NextRound(SpriteAnimation firstSprite, SpriteAnimation secondSprite)
        {
            if (firstWin == 2 || secondWin == 2)
            {
                CurrentRound = 1;
                Routine(firstSprite, secondSprite);
                ShowWinner = false;
                return false;
            }
            CurrentRound += 1;
            Routine(firstSprite, secondSprite);
            ShowWinner = true;
            return true;
        }

        public static SpriteAnimation GetWinner(SpriteAnimation first, SpriteAnimation second)
        {
            if (firstWin == 2)
                return first;
            return second;
        }

        private static void Routine(SpriteAnimation firstSprite, SpriteAnimation secondSprite)
        {
            firstSprite.Information.Health = GameSettings.MaxHealth;
            firstSprite.Position = new Vector2(GameSettings.FirstPosition.X - 100, GameSettings.FirstPosition.Y);
            firstSprite.Routine();
            secondSprite.Information.Health = GameSettings.MaxHealth;
            secondSprite.Position = new Vector2(GameSettings.FirstPosition.X + 100, GameSettings.FirstPosition.Y);
            secondSprite.Routine();
        }

    }
}
