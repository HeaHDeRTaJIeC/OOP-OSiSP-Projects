using System;
using AbstructClasses;

namespace MainClassLibrary
{
    [Serializable]
    public class AutomaticRifles : Firearm
    {
        public AutomaticRifles() 
        {
            Bullet = new Bullets();
            Random rand = new Random();
            int a = rand.Next(1000);
            if (a % 2 == 1)
            {
                Silencer = true;
                Bayonet = false;
            }
            else
            {
                Silencer = false;
                Bayonet = true;
            }
            NBulletsPerShot = a % 3 + 1;
            NumberOfBullets = a % 30 + 10;
            Bullet.Caliber = rand.NextDouble() * 10;

        }
        public bool Silencer { get; set; }  //Глушитель
        public bool Bayonet { get; set; }   //Штык-нож
        public TypeOfGrenade Grenade { get; set; }
        public FiringMode Mode { get; set; }
        public int NBulletsPerShot { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
