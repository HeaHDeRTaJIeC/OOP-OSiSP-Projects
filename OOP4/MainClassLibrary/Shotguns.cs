using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class Shotguns : Firearm
    {
        public Shotguns() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);

            NumberOfBullets = a % 30 + 10;
            Bullet.Caliber = rand.NextDouble() * 10;
        }
        public TypeOfShotReload ShotReload { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
