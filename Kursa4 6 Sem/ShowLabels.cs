using System;
namespace MortalKombatXI
{
    class ShowLabels
    {
        private static int intervalWinner;
        private static int intervalRound;
        private static int intervalEndGame;

        public static void ShowRound()
        {
            if (GameState.ShowRound && intervalRound < 100)
            {
                intervalRound++;
            }
            else
            {
                if (intervalRound == 100)
                {
                    intervalRound = 0;
                    GameState.ShowRound = false;
                }
            }
        }

        public static void ShowWinner()
        {
            if (GameState.ShowWinner && intervalWinner < 100)
            {
                intervalWinner++;
            }
            else
            {
                if (intervalWinner == 100)
                {
                    intervalWinner = 0;
                    GameState.ShowWinner = false;
                    GameState.ShowRound = true;
                }
            }
        }

        public static void ShowEndGame()
        {
            intervalEndGame++;
            if (intervalEndGame == 200)
            {
                intervalEndGame = 0;
                GameState.IsEnd = false;
                GameState.ShowMenu = true;
            }
        }
    }
}
