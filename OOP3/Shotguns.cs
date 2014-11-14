using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP3;

namespace MainClassLibrary
{
    public enum TypeOfShotReload
    {
        Manual,
        Automatic
    }

    [Serializable]
    public class Shotguns : Firearm
    {
        public Shotguns() { Bullet = new Bullets(); }
        public TypeOfShotReload ShotReload { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
