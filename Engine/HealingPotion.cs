using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // convert HealingPotion into a child class
    public class HealingPotion : Item
    {
        // Add class propperties
        public int AmountToHeal { get; set; }
    }
}
