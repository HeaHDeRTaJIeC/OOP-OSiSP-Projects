using System;

namespace AbstructClasses
{
    [Serializable]
    abstract public class Firearm : Weapons
    {
        public TypeOfReload ReloadMechanism { get; set; }
        public TypeOfAim Aim { get; set; }
        public int BulletSpeed { get; set; }
    }
}
