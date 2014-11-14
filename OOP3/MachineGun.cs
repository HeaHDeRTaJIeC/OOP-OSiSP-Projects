using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP3;

namespace MainClassLibrary
{
    [Serializable]
    public class MachineGun : Firearm
    {
        public MachineGun() { Bullet = new Bullets(); }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
