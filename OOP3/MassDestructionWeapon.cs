using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    public enum TypeOfDetonation
    {
        Manual,
        Automatical
    }

    public enum TypeOfInfluence
    {
        Nuclear,
        Biological,
        Chemical
    }

    [Serializable]
    abstract public class MassDestructionWeapon : Weapons
    {
        public int RangeOfDamage;
        public TypeOfDetonation Detonation;
        public TypeOfInfluence Influence;
    }
}
