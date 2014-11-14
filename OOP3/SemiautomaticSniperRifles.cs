using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP3;

namespace MainClassLibrary
{
    [Serializable]
    public class SemiautomaticSniperRifles : SniperRifles
    {
        public SemiautomaticSniperRifles() { Bullet = new Bullets(); }
        public int NumberOfBullets { get; set; }
        public TypeOfShotReload FireMode { get; set; }
    }
}
