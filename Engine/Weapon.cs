using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        // add Weapon properties

        public string Category { get; set; }
        public int Damage { get; set; }
        public string AmmoType { get; set; }
    }
}
