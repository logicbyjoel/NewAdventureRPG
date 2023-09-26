using System;
using System.Collections.Generic;   // work with collections
using System.Linq;
using System.Xml;   // work with XML functions
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
        public int Level {
            // calculate a player's level and start at level 1
            get
            {   return ((ExperiencePoints / 100) + 1); }
            // remove 'set'. This is because we will never put a value into the Level property
            // NOTE: removing 'set' makes a property read-only
            //set; 
        }

        // prop for updating the player's location at all times
        public Location CurrentLocation { get; set; }

        // declare props for InventoryItem, LootItem, PlayerQuest, and QuestCompletionItem classes
        // to hold lists contatining InventoryItem and PlayerQuest objects
        public List<InventoryItem> Inventory { get; set; }
        public List<PlayerQuest> Quests { get; set; }


        /// <summary>
        /// add param constructor and reference base class props
        /// convert Player constructor from public to private after addition of XML for storing player's data
        /// this means that the constructor can only be called by anothyer function inside this Player class. 
        ///NOTE: this is not absolutely necessary, but we are going to use other two methods to create a Player object, we made the constructor private meaning..
        /// it can only be called by another function inside this Player class. 
        /// </summary>
        /// <param name="currentHitPoints"></param>
        /// <param name="maximumHitPoints"></param>
        /// <param name="gold"></param>
        /// <param name="experiencePoints"></param>
        private Player(int currentHitPoints, int maximumHitPoints, int gold, int experiencePoints) : base(currentHitPoints, maximumHitPoints)
        {
            Gold = gold;
            ExperiencePoints = experiencePoints;
            // remove Level, because we removed the 'setter' from this property
            // Level = level;

            Inventory = new List<InventoryItem>();
            Quests = new List<PlayerQuest>();
        }

        // After addition of XML for storing player's data, we will create a default player object in case player's data cannot be retrieved
        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(10, 10, 20, 0);
            player.Inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            player.CurrentLocation = World.LocationByID(World.LOCATION_ID_HOME);
            return player;
        }   // end CreateDefaultPlayer()

        public static Player CreatePlayerFromXmlString(string xmlPlayerData)
        {
            try
            {
                XmlDocument playerData = new XmlDocument();
                playerData.LoadXml(xmlPlayerData);
                int currentHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/Gold").InnerText);
                int experiencePoints = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints);
                int currentLocationID = Convert.ToInt32(playerData.SelectSingleNode("/Player/Stats/CurrentLocation").InnerText);
                player.CurrentLocation = World.LocationByID(currentLocationID);

                foreach(XmlNode node in playerData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    for(int i = 0; i < quantity; i++)
                    {
                        player.AddItemToInventory(World.ItemByID(i));
                    }
                }

                foreach(XmlNode node in playerData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    bool isCompleted = Convert.ToBoolean(node.Attributes["IsCompleted"].Value);
                    PlayerQuest playerQuest = new PlayerQuest(World.QuestByID(id));
                    playerQuest.IsCompleted = isCompleted;
                    player.Quests.Add(playerQuest);
                }
                return player;
            }
            catch
            {
                // if there is an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }

        // start refactoring code from SuperAdventure.cs > MyAdventureRPG SuperAdventure > MoveTo()
        public bool HasRequiredItemToEnterThisLocation(Location destination)
        {
            if(destination.ItemRequiredToEnter == null)
            {
                // no required item for this location, return true
                return true;
            }

            // Change: use LINQ to check if the player has the location-specific required item in their inventory
            // NOTE: Exists() will check the items in specified list, to find any items matching the expression given. 
            // if the item is found, will return true; false otherwise
            // NOTE: this is a lambda expression being used
            return Inventory.Exists(ii => ii.Details.ID == destination.ItemRequiredToEnter.ID);


        }   // end HasRequiredItemToEnterThisLocation()

        public bool HasThisQuest(Quest quest)
        {
            // find the quest for this destination
            return Quests.Exists(pq => pq.Details.ID == quest.ID);
        }   // end HasThisQuest()

        public bool CompletedThisQuest(Quest quest)
        {
            // check if qauest has been completed
            foreach(PlayerQuest playerQuest in Quests)
            {
                // find matching quest to current quest
                if (playerQuest.Details.ID == quest.ID)
                {
                    // return bool of player's quest
                    return playerQuest.IsCompleted;
                }
            }
            return false;
        }

        public bool HasAllQuestCompletionItems(Quest quest)
        {
            // check if player has all quest completion items required for this quest
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                // assume item will not be found
                //bool foundItemInPlayersInventory = false;

                // check each item in player's inv, to see if they have it, and the minimum quanity
                // refactor using LINQ
                /*foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID) // player has the item in inventory
                    {
                        foundItemInPlayersInventory = true;

                        if(ii.Quantity < qci.Quantity)  // player does not have enough of this item to complete the quest
                        {
                            return false;
                        }
                    }
                }*/

                if(!Inventory.Exists(ii => ii.Details.ID == qci.Details.ID && ii.Quantity >= qci.Quantity))
                {
                    return false;
                }
                // player does not have any of this quest completion items in their inventory
                /* if (!foundItemInPlayersInventory)
                {
                    return false;
                }*/
            }

            // if we made it here, the player must hnave all the required items, and enough of them, to complete the quest
            return true;
        }   // end HasAllQuestCompletionItems()

        public void RemoveQuestCompletionItems(Quest quest)
        {
            // find all quest completion items to remove from inventory
            foreach (QuestCompletionItem qci in quest.QuestCompletionItems)
            {
                foreach (InventoryItem ii in Inventory)
                {
                    if(ii.Details.ID == qci.Details.ID)
                    {
                        // subtract  the quantity from the play'ers inventory that was needed to complete the quest
                        ii.Quantity -= qci.Quantity;
                        break;
                    }
                }
            }
        }   // end RemoveQuestCompletionItems()

        public void AddItemToInventory(Item itemToAdd)
        {
            foreach (InventoryItem ii in Inventory)
            {
                // check for a matching item in inventory to add to its quantity
                if(ii.Details.ID == itemToAdd.ID)
                {
                    // item of same id is found in inventory, increment quantity
                    ii.Quantity++;

                    return; // item was added, exit the function
                }
            }
            // item was not found, add this item to their inventory as a new item, with quantity of 1
            Inventory.Add(new InventoryItem(itemToAdd, 1));
        }   // end AddItemToInventory()

        public void MarkQuestCompleted(Quest quest)
        {
            // check player's quest to locate the quest in question
            foreach (PlayerQuest playerQuest in Quests)
            {
                if(playerQuest.Details.ID == quest.ID)
                {
                    playerQuest.IsCompleted = true;
                    return; // we found teh quest, and marked it as complet. exit the function
                }
            }
        }   // end MarkQuestCompleted()

        public string ToXmlString()
        {
            XmlDocument playerData = new XmlDocument();
            // create the top-level XML node
            XmlNode player = playerData.CreateElement("Player");
            playerData.AppendChild(player);
            // create the 'stats' child node to hold the other player statistics nodes
            XmlNode stats = playerData.CreateElement("Stats");
            player.AppendChild(stats);
            // create the child nodes for the "Stats" node
            XmlNode currentHitPoints = playerData.CreateElement("CurrentHitPoints");

            currentHitPoints.AppendChild(playerData.CreateTextNode(this.CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);
            XmlNode maximumHitPoints = playerData.CreateElement("MaximumHitPoints");

            maximumHitPoints.AppendChild(playerData.CreateTextNode(this.MaximumHitPoints.ToString()));
            
            stats.AppendChild(maximumHitPoints);
            XmlNode gold = playerData.CreateElement("Gold");
            gold.AppendChild(playerData.CreateTextNode(this.Gold.ToString()));
            stats.AppendChild(gold);
            XmlNode experiencePoints = playerData.CreateElement("ExperiencePoints");

            experiencePoints.AppendChild(playerData.CreateTextNode(this.ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);
            XmlNode currentLocation = playerData.CreateElement("CurrentLocation");

            currentLocation.AppendChild(playerData.CreateTextNode(this.CurrentLocation.ToString()));
            stats.AppendChild(currentLocation);

            // Create the "InventoryITems" child node to hold each InventoryIteem node
            XmlNode inventoryItems = playerData.CreateElement("InventoryItems");

            // create the "InventoryItem" node for each item in the player's invneotory
            foreach (InventoryItem ii in this.Inventory)
            {
                // wrong approach
                //XmlNode item = playerData.CreateElement("item" +ii.index +" ");

                XmlNode inventoryItem = playerData.CreateElement("InventoryItem");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = ii.Details.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);
                XmlAttribute quantityAttribute = playerData.CreateAttribute("Quantity");
                quantityAttribute.Value = ii.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);
                inventoryItems.AppendChild(inventoryItem);
            }

            // ccreate the "Player Quests" child node to hold each PlayerQuest node
            XmlNode playerQuests = playerData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            // create the "PlayerQuest" node for each quest the player has acquired
            foreach(PlayerQuest pq in Quests)
            {
                XmlNode playerQuest = playerData.CreateElement("PlayerQuest");
                XmlAttribute idAttribute = playerData.CreateAttribute("ID");
                idAttribute.Value = pq.Details.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);
                XmlAttribute isCompletedAttribute = playerData.CreateAttribute("IsCompleted");
                isCompletedAttribute.Value = pq.IsCompleted.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);
                playerQuests.AppendChild(playerQuest);
            }
            return playerData.InnerXml; // the XML document, as a string, so we can save the data to disk
        }   // end ToXmlString()


    }
}
