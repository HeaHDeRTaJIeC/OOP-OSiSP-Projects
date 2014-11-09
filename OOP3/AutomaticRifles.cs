using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    public enum TypeOfGrenade
    {
        None,
        Smoke,
        Stun,
        Bursting
    }

    public enum FiringMode             //Режим стрельбы(одиночные, очереди, по несколько патронов)
    {
        Single,
        Queues,
        NBulletsPerShot
    }

    [Serializable]
    public class AutomaticRifles : Firearm
    {
        public AutomaticRifles() { Bullet = new Bullets(); }
        public bool Silencer { get; set; }  //Глушитель
        public bool Bayonet { get; set; }   //Штык-нож
        public TypeOfGrenade Grenade { get; set; }
        public FiringMode Mode { get; set; }
        public int NBulletsPerShot { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
