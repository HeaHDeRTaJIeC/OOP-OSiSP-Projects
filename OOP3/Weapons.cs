using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    [Serializable]
    abstract public class Weapons : Object
    {
        public string WeaponTitle { get; set; }
        public int Effectivedistance { get; set; }
    }
}
