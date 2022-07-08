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
        public Boss(int id, string name, int maximumDamage, int rewardExperiencePoints, int rewardGold, int currentHitPoints, int maximumHitPoints) : base(currentHitPoints, maximumHitPoints)
        {
            ID = id;
            Name = name;
            MaximumDamage = maximumDamage;
            RewardExperiencePoints = rewardExperiencePoints;
            RewardGold = rewardGold;
            // instantiate table of loot for Boss
            // NOTE: lists are null until they are set to an empty list
            LootTable = new List<LootItem>();

        }
        // add Boss properties
        public int ID { get; set; }
        public string Name { get; set; }

        public int MaximumDamage { get; set; }
        public int RewardExperiencePoints { get; set; }
        public int RewardGold { get; set; }

        // add a loot table for the boss
        public List<LootItem> LootTable { get; set; }
    }
}
