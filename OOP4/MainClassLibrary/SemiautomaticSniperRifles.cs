using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class SemiautomaticSniperRifles : SniperRifles
    {
        public SemiautomaticSniperRifles() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);

            NumberOfBullets = a % 30 + 10;
            Bullet.Caliber = rand.NextDouble() * 10;
        }
        public int NumberOfBullets { get; set; }
        public TypeOfShotReload FireMode { get; set; }
        public Bullets Bullet { get; set; }
    }
}
