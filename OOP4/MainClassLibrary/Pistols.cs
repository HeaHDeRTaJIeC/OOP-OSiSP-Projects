using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class Pistols : Firearm
    {
        public Pistols() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);

            Silencer = a % 2 == 1;
            NumberOfBullets = a % 30 + 10;
            Bullet.Caliber = rand.NextDouble() * 10;
        }
        public bool Silencer { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
