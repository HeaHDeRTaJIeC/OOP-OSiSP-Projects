using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class SniperRifles : Firearm
    {
        public SniperRifles() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);

            Silencer = a % 2 == 1;
            Zoom = a % 4 + 2;
            Bullet.Caliber = rand.NextDouble() * 10;
        }
        public bool Silencer { get; set; }
        public int Zoom { get; set; }
        public Bullets Bullet { get; set; }
    }
}
