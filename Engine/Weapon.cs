using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        // add param constructor and reference the base class props
        public Weapon(int id, string name, string namePlural, int minimumDamage, int maximumDamage) : base(id, name, namePlural)
        {
            MaximumDamage = maximumDamage;
            MinimumDamage = minimumDamage;
        }
        // add Weapon properties

        public int MaximumDamage { get; set; }
        public int MinimumDamage { get; set; }
/*        public string Category { get; set; }
        public int Damage { get; set; }
        public string AmmoType { get; set; }*/
    }
}
