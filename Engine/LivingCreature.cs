using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LivingCreature
    {

        // add param constructor for this base class
        public LivingCreature(int currentHitPoints, int maximumHitPoints)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
        }
        // add properties for LivingCreature

        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; set; }

    }
}
