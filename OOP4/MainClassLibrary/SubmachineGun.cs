using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class SubmachineGun : Pistols
    {
        public SubmachineGun() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);

            NumberOfBullets = a % 30 + 10;
            Bullet.Caliber = rand.NextDouble() * 10;
        }
        public FiringMode Mode { get; set; }
        public int NBulletsPerShot { get; set; }
    }
}
