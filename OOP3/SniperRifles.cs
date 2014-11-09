using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    [Serializable]
    public class SniperRifles : Firearm
    {
        public SniperRifles() { Bullet = new Bullets(); }
        public bool Silencer { get; set; }
        public int Zoom { get; set; }
        public Bullets Bullet { get; set; }
    }
}
