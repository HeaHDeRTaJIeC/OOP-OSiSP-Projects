using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    public enum TypeOfReload
    {
        MagazineReload,
        PrechargeReload
    }

    public enum TypeOfAim 
    {
        Optical,
        Mechanical,
        Collimator,
        Holographic,
        Laser
    }

    [Serializable]
    abstract public class Firearm : Weapons
    {
        public TypeOfReload ReloadMechanism { get; set; }
        public TypeOfAim Aim { get; set; }
        public int BulletSpeed { get; set; }
    }
}
