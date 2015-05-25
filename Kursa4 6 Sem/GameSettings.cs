using System;
using Microsoft.Xna.Framework;

namespace MortalKombatXI
{
    class GameSettings
    {
        public static int Step = 12;
        public static float AnimationInterval = 90f;
        public static int SpriteWidth = 200;
        public static int SpriteHeigth = 200;
        public static Point FirstPosition = new Point();
        //public static bool FightStart;
        public static int MaxHealth = 10;
        public static String firstName = "Scorpion";
        public static String secondName = "SubZero";
        public static String firstMeasure = "Scorpi";
        public static String secondMeasure = "SubZe";
        public static Vector2 HealthBar = new Vector2(400, 100);
        public static Vector2 NameWinBar = new Vector2(400, 150);
        public static Color menuColor = Color.DarkRed;
        public static Color activeColor = Color.DarkGray;
    }
    
}
