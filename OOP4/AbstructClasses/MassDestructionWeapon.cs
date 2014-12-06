namespace AbstructClasses
{
    abstract public class MassDestructionWeapon : Weapons
    {
        public int RangeOfDamage { get; set; }
        public TypeOfDetonation Detonation { get; set; }
        public TypeOfInfluence Influence { get; set; }
    }
}
