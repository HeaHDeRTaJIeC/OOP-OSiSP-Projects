using System;

namespace AbstructClasses
{
    [Serializable]
    abstract public class Weapons : Object
    {
        public string WeaponTitle { get; set; }
        public int Effectivedistance { get; set; }
    }
}
