using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Boss : LivingCreature
    {
        // add param constructor and reference base class props
        public Boss(int currentHitPoints, int maximumHitPoints, int id, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold) : base(currentHitPoints, maximumHitPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;

        }
        // add Boss properties
        public int ID { get; set; }
        public string Name { get; set; }

        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }
    }
}
