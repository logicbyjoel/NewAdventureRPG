using System;
using System.Collections.Generic;   // work with collections
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    // change access modifer to public
    public class Player : LivingCreature
    {
        // declare Player properties 
        // NOTE: public properties use Pascal casing

        public int Gold { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }

        // prop for updating the player's location at all times
        public Location CurrentLocation { get; set; }

        // declare props for InventoryItem, LootItem, PlayerQuest, and QuestCompletionItem classes
        // to hold lists contatining InventoryItem and PlayerQuest objects
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }

        // add param constructor and reference base class props
        public Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints, int level) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();

        }

        // start refactoring code from MoveTo()
        public bool HasRequiredItemToEnterThisLocation(Location location)
        {

        }   // end HasRequiredItemToEnterThisLocation()

        public bool HasThisQuest(Quest quest)
        {

        }

        public bool CompletedThisQuest(Quest quest)
        {

        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {

        }

        public void RemoveQuestCompletionItems(Quest quest)
        {

        }

        public void AddItemToInventory(Item itemToAdd)
        {

        }

        public void MarkQuestCompleted(Quest quest)
        {

        }
    }
}
