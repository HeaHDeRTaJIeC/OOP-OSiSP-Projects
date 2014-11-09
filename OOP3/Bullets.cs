using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP3
{
    public enum TypeOfBullet
    {
        Usual,                  //Обычная
        Shot,                   //Дробь
        ArmorPiercing,          //Бронебойный
        Incendiary,             //Зажигательный
        Explosive,              //Разрывная
    }

    [Serializable]
    public class Bullets
    {
        public Bullets() {}
        public double Caliber { get; set; }
        public TypeOfBullet Type { get; set; }
    }
}
