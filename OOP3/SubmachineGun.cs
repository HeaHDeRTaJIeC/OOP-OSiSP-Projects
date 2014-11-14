using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP3;

namespace MainClassLibrary
{
    [Serializable]
    public class SubmachineGun : Pistols
    {
        public SubmachineGun() { Bullet = new Bullets(); }
        public FiringMode Mode { get; set; }
        public int NBulletsPerShot { get; set; }
    }
}
