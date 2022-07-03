using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // change access modifer to public
    public class Player
    {
        // declare Player properties 
        // NOTE: public properties use Pascal casing
        public int CurrentHitPoints { get; set; }
        public int MaximumHitPoints { get; set; }
        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
    }
}
