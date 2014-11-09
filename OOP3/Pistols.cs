using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    [Serializable]
    public class Pistols : Firearm
    {
        public Pistols() { Bullet = new Bullets(); }
        public bool Silencer { get; set; }
        public int NumberOfBullets { get; set; }
        public Bullets Bullet { get; set; }
    }
}
